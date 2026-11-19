using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Resilience
{
    /// <summary>QG-611: AddHttpClient должен нести политику отказоустойчивости (retry/circuit-breaker).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class HttpClientResiliencePolicyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG611";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Политика отказоустойчивости обязательна",
            "AddHttpClient должен добавлять политику отказоустойчивости (AddPolicyHandler/AddResilienceHandler). Используйте AddResilientHttpClient из DigiTFactory.Libraries.CrossCutting (QG-611)",
            "DigiTFactory",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeInvocation(SyntaxNodeAnalysisContext ctx)
        {
            var inv = (InvocationExpressionSyntax)ctx.Node;
            if (MethodName(inv) != "AddHttpClient")
            {
                return;
            }

            var outermost = inv.Ancestors().OfType<InvocationExpressionSyntax>().LastOrDefault() ?? inv;
            var hasPolicy = outermost.DescendantNodesAndSelf()
                .OfType<InvocationExpressionSyntax>()
                .Any(x =>
                {
                    var name = MethodName(x);
                    return name == "AddPolicyHandler" || name == "AddResilienceHandler";
                });

            if (!hasPolicy)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, inv.GetLocation()));
            }
        }

        private static string? MethodName(InvocationExpressionSyntax inv)
            => (inv.Expression as MemberAccessExpressionSyntax)?.Name.Identifier.ValueText;
    }
}
