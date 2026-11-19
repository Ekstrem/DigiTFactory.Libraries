using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Resilience
{
    /// <summary>QG-609: нельзя создавать HttpClient напрямую (исчерпание сокетов) — только IHttpClientFactory.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoNewHttpClientAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG609";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "new HttpClient() запрещён",
            "Создавать HttpClient напрямую запрещено (исчерпание сокетов). Используйте IHttpClientFactory/AddHttpClient (QG-609)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
        }

        private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext ctx)
        {
            var oce = (ObjectCreationExpressionSyntax)ctx.Node;
            var typeName = oce.Type.ToString();
            if (typeName == "HttpClient" || typeName.EndsWith(".HttpClient"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, oce.GetLocation()));
            }
        }
    }
}
