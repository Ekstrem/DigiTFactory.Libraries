using System.Reflection;
using System.Runtime.Loader;
using Xunit;
using Xunit.Abstractions;

namespace DigiTFactory.Libraries.TestRunner;

/// <summary>
/// Minimal in-process xUnit test runner. It discovers and executes tests in the current
/// process via <see cref="XunitFrontController"/> with <see cref="AppDomainSupport.Denied"/>,
/// which avoids spawning VSTest/testhost (and the Process.OpenProcess Win32Exception 5 that
/// occurs in the DSH sandbox).
/// ВНИМАНИЕ: это ФОЛБЭК, не канон. Санкционированный локальный прогон — настоящий
/// xunit.console (xunit.runner.console) по собранным артефактам; результаты этого
/// раннера помечать как «неофициальные» (см. library-known-issues S6).
/// </summary>
internal static class Program
{
    private const int ExitAllPassed = 0;
    private const int ExitTestFailures = 1;
    private const int ExitRunnerError = 2;

    private static readonly TimeSpan PhaseTimeout = TimeSpan.FromMinutes(10);

    public static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return ExitRunnerError;
        }

        string assemblyFileName;
        try
        {
            assemblyFileName = Path.GetFullPath(args[0]);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"error: invalid assembly path '{args[0]}': {ex.Message}");
            return ExitRunnerError;
        }

        if (!File.Exists(assemblyFileName))
        {
            Console.Error.WriteLine($"error: test assembly not found: {assemblyFileName}");
            return ExitRunnerError;
        }

        try
        {
            Console.WriteLine($"Test assembly: {assemblyFileName}");

            InstallAssemblyResolver(assemblyFileName);

            using var controller = new XunitFrontController(
                AppDomainSupport.Denied,
                assemblyFileName,
                configFileName: null,
                shadowCopy: false,
                diagnosticMessageSink: new ConsoleDiagnosticSink());

            // --- Discovery -------------------------------------------------------------
            // Note: Find() queues work on an xUnit worker thread and returns immediately.
            var discoverySink = new DiscoverySink();
            controller.Find(includeSourceInformation: false, messageSink: discoverySink, discoveryOptions: TestFrameworkOptions.ForDiscovery());
            if (!discoverySink.Finished.Wait(PhaseTimeout))
            {
                Console.Error.WriteLine("error: discovery timed out.");
                return ExitRunnerError;
            }

            var testCases = discoverySink.TestCases;
            Console.WriteLine($"Discovered {testCases.Count} test case(s).");

            // --- Execution --------------------------------------------------------------
            // Note: RunTests() is asynchronous as well; completion is signaled by ITestAssemblyFinished.
            var executionSink = new ExecutionSink();
            controller.RunTests(testCases, executionSink, TestFrameworkOptions.ForExecution());
            if (!executionSink.Finished.Wait(PhaseTimeout))
            {
                Console.Error.WriteLine("error: test execution timed out.");
                return ExitRunnerError;
            }

            return Report(executionSink);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"error: test run failed: {ex}");
            return ExitRunnerError;
        }
    }

    /// <summary>
    /// Since we run the test assembly in-process (<see cref="AppDomainSupport.Denied"/>), the
    /// test framework engine (<c>xunit.execution.dotnet</c>, <c>xunit.core</c>, <c>xunit.assert</c>)
    /// and every dependency of the test assembly must be resolvable from the test assembly's
    /// output directory. This hook loads them from there.
    /// </summary>
    private static void InstallAssemblyResolver(string testAssemblyPath)
    {
        var dependencyResolver = new AssemblyDependencyResolver(testAssemblyPath);
        var testDirectory = Path.GetDirectoryName(testAssemblyPath);

        AssemblyLoadContext.Default.Resolving += (context, name) =>
        {
            // 1) Resolve through the test assembly's .deps.json (handles the transitive graph).
            var path = dependencyResolver.ResolveAssemblyToPath(name);
            if (path != null)
            {
                return context.LoadFromAssemblyPath(path);
            }

            // 2) Fall back to a flat probe of the test assembly's directory.
            if (testDirectory != null)
            {
                var candidate = Path.Combine(testDirectory, name.Name + ".dll");
                if (File.Exists(candidate))
                {
                    return context.LoadFromAssemblyPath(candidate);
                }
            }

            return null;
        };
    }

    private static int Report(ExecutionSink sink)
    {
        Console.WriteLine();
        Console.WriteLine(
            $"Summary: total={sink.Total}, passed={sink.Passed}, failed={sink.Failed}, skipped={sink.Skipped}.");

        if (sink.Failures.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Failed tests:");
            foreach (var failure in sink.Failures)
            {
                Console.WriteLine($"  [FAIL] {failure.Name}");
                for (var i = 0; i < failure.Messages.Count; i++)
                {
                    var exceptionType = i < failure.ExceptionTypes.Count ? failure.ExceptionTypes[i] : string.Empty;
                    Console.WriteLine($"    {exceptionType}: {failure.Messages[i]}");
                    if (i < failure.StackTraces.Count)
                    {
                        Console.WriteLine($"    stack: {failure.StackTraces[i]}");
                    }
                }
            }
        }

        return sink.Failed == 0 ? ExitAllPassed : ExitTestFailures;
    }

    private static void PrintUsage()
    {
        Console.Error.WriteLine("Usage: DigiTFactory.Libraries.TestRunner <test-assembly.dll>");
        Console.Error.WriteLine();
        Console.Error.WriteLine("Runs xUnit tests in-process (no VSTest/testhost) and exits with:");
        Console.Error.WriteLine($"  {ExitAllPassed} - all tests passed");
        Console.Error.WriteLine($"  {ExitTestFailures} - at least one test failed or errored");
        Console.Error.WriteLine($"  {ExitRunnerError} - usage or startup error");
    }
}

/// <summary>
/// Collects discovered test cases and signals completion when <see cref="IDiscoveryCompleteMessage"/>
/// is received (the message xUnit sends at the end of discovery).
/// </summary>
internal sealed class DiscoverySink : IMessageSink
{
    public List<ITestCase> TestCases { get; } = new();

    public ManualResetEventSlim Finished { get; } = new(initialState: false);

    public bool OnMessage(IMessageSinkMessage message)
    {
        if (message is ITestCaseDiscoveryMessage discovery)
        {
            TestCases.Add(discovery.TestCase);
        }

        if (message is IDiscoveryCompleteMessage)
        {
            Finished.Set();
        }

        return true;
    }
}

/// <summary>
/// Counts test results, records failures, and signals completion when
/// <see cref="ITestAssemblyFinished"/> is received (the message xUnit sends at the end of execution).
/// </summary>
internal sealed class ExecutionSink : IMessageSink
{
    private int _passed;
    private int _failed;
    private int _skipped;
    private readonly List<TestFailure> _failures = new();

    public int Passed => _passed;
    public int Failed => _failed;
    public int Skipped => _skipped;
    public int Total => _passed + _failed + _skipped;
    public IReadOnlyList<TestFailure> Failures => _failures;

    public ManualResetEventSlim Finished { get; } = new(initialState: false);

    public bool OnMessage(IMessageSinkMessage message)
    {
        if (message is ITestPassed)
        {
            _passed++;
        }
        else if (message is ITestFailed failed)
        {
            _failed++;
            _failures.Add(new TestFailure(
                failed.Test?.DisplayName ?? "<unknown>",
                failed.ExceptionTypes ?? Array.Empty<string>(),
                failed.Messages ?? Array.Empty<string>(),
                failed.StackTraces ?? Array.Empty<string>()));
        }
        else if (message is ITestSkipped)
        {
            _skipped++;
        }
        else if (message is ITestAssemblyFinished)
        {
            Finished.Set();
        }

        return true;
    }
}

/// <summary>Forwards xUnit diagnostic messages to the console.</summary>
internal sealed class ConsoleDiagnosticSink : IMessageSink
{
    public bool OnMessage(IMessageSinkMessage message)
    {
        if (message is IDiagnosticMessage diagnostic)
        {
            Console.WriteLine($"[diagnostic] {diagnostic.Message}");
        }

        return true;
    }
}

/// <summary>Details of a single failed test.</summary>
internal sealed record TestFailure(
    string Name,
    IReadOnlyList<string> ExceptionTypes,
    IReadOnlyList<string> Messages,
    IReadOnlyList<string> StackTraces);
