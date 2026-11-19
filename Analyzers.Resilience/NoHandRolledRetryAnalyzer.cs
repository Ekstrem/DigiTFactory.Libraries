using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Resilience
{
    /// <summary>QG-613: самописный retry (цикл + Sleep/Delay) запрещён — используй Polly.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoHandRolledRetryAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG613";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Самописный retry запрещён",
            "Самописный цикл повторных попыток с Sleep/Delay запрещён — используй Polly (QG-613)",
            "DigiTFactory",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeLoop, SyntaxKind.WhileStatement);
            context.RegisterSyntaxNodeAction(AnalyzeLoop, SyntaxKind.ForStatement);
            context.RegisterSyntaxNodeAction(AnalyzeLoop, SyntaxKind.DoStatement);
        }

        private static void AnalyzeLoop(SyntaxNodeAnalysisContext ctx)
        {
            var hasDelay = ctx.Node.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(x =>
                {
                    var name = (x.Expression as MemberAccessExpressionSyntax)?.Name.Identifier.ValueText;
                    return name == "Sleep" || name == "Delay";
                });

            if (hasDelay)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, ctx.Node.GetLocation()));
            }
        }
    }
}
