using System.Collections.Immutable;
using DigiTFactory.Libraries.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

// DtfScan — прогон Quality Gates (Roslyn-анализаторы) по дереву исходников.
//   usage: DtfScan [root] [refDir]
//   root   — корень с .cs (по умолчанию XDiary/6-msa)
//   refDir — папка reference assemblies (ref pack .NET), для symbol-анализаторов.

string root = args.Length > 0
    ? args[0]
    : @"C:\Users\About\Documents\GitHub\XDiary\6-msa";
string refDir = args.Length > 1
    ? args[1]
    : @"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.25\ref\net8.0";

if (!Directory.Exists(root))
{
    Console.Error.WriteLine($"root not found: {root}");
    return 2;
}

var files = Directory.EnumerateFiles(root, "*.cs", SearchOption.AllDirectories)
    .Where(f => !f.Replace('\\', '/').Contains("/bin/") && !f.Replace('\\', '/').Contains("/obj/"))
    .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
    .ToList();

var trees = files
    .Select(f => (SyntaxTree)CSharpSyntaxTree.ParseText(File.ReadAllText(f), path: f))
    .ToList();

var references = Directory.Exists(refDir)
    ? Directory.EnumerateFiles(refDir, "*.dll")
        .Select(d => (MetadataReference)MetadataReference.CreateFromFile(d))
        .ToList()
    : new List<MetadataReference>();

var compilation = CSharpCompilation.Create(
    "DtfScan",
    trees,
    references,
    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

var analyzers = new DiagnosticAnalyzer[]
{
    new NoPutDeleteAnalyzer(),
    new NoNotImplementedAnalyzer(),
    new NoAbstractAggregateAnalyzer(),
    new NoPublicSetterAnalyzer(),
    new NoCustomInfrastructureAnalyzer(),
    new NoMutableAnemicModelAnalyzer(),
    new NoMutableValueObjectAnalyzer(),
    new AggregateFactoryAnalyzer(),
};

var withAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzers));
var diagnostics = (await withAnalyzers.GetAnalyzerDiagnosticsAsync())
    .OrderBy(d => d.Id, StringComparer.Ordinal)
    .ThenBy(d => d.Location.SourceTree?.FilePath ?? string.Empty, StringComparer.OrdinalIgnoreCase)
    .ToList();

Console.WriteLine("== DtfScan (DigiTFactory Quality Gates) ==");
Console.WriteLine($"root      : {root}");
Console.WriteLine($"files     : {files.Count}");
Console.WriteLine($"violations: {diagnostics.Count}");
Console.WriteLine();

foreach (var group in diagnostics.GroupBy(d => d.Id))
{
    Console.WriteLine($"[{group.Key}]  {group.Count()}");
    foreach (var d in group)
    {
        var span = d.Location.GetLineSpan();
        var path = span.Path;
        var display = path.Length > root.Length && path.StartsWith(root)
            ? path.Substring(root.Length + 1)
            : path;
        Console.WriteLine($"  {display}:{span.StartLinePosition.Line + 1}  {d.GetMessage()}");
    }
    Console.WriteLine();
}

return diagnostics.Count > 0 ? 1 : 0;
