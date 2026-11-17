using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.CrossCutting
{
    /// <summary>QG-603: в продуктовом коде нет заглушек NotImplementedException.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoNotImplementedAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG603";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Заглушки запрещены",
            "NotImplementedException в продуктовом коде запрещён (QG-603)",
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
            if (typeName == "NotImplementedException" || typeName.EndsWith(".NotImplementedException"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, oce.GetLocation()));
            }
        }
    }
}
