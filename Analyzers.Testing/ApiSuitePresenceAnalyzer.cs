using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Testing
{
    /// <summary>
    /// QG-706: *.IntegrationTests обязан содержать self-hosted API suite
    /// (минимум один WebApplicationFactory-тест). Проверяется наличие, а не поведение.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ApiSuitePresenceAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG706";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Нет self-hosted API suite",
            "Добавь в {0} self-hosted API suite на WebApplicationFactory: каждый command/query endpoint — happy + негатив (401/403/405/malformed), owner из token (QG-706)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationAction(AnalyzeCompilation);
        }

        private static void AnalyzeCompilation(CompilationAnalysisContext ctx)
        {
            var assemblyName = ctx.Compilation.AssemblyName ?? string.Empty;
            if (!assemblyName.EndsWith(".IntegrationTests"))
            {
                return;
            }

            var hasWebApplicationFactory = ctx.Compilation.SyntaxTrees
                .SelectMany(t => t.GetRoot().DescendantNodes())
                .OfType<IdentifierNameSyntax>()
                .Any(id => id.Identifier.ValueText == "WebApplicationFactory");

            if (!hasWebApplicationFactory)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, Location.None, assemblyName));
            }
        }
    }
}
