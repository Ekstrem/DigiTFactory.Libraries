using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Resilience
{
    /// <summary>QG-610: EF-подключение должно быть устойчивым — UseNpgsql/UseSqlServer с EnableRetryOnFailure.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class EfEnableRetryOnFailureAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG610";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Устойчивое EF-подключение обязательно",
            "UseNpgsql/UseSqlServer должно настраивать EnableRetryOnFailure (QG-610)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
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
            var name = MethodName(inv);
            if (name != "UseNpgsql" && name != "UseSqlServer" && name != "UseSqlite")
            {
                return;
            }

            var lambda = inv.ArgumentList.Arguments
                .Select(a => a.Expression)
                .OfType<LambdaExpressionSyntax>()
                .FirstOrDefault();
            if (lambda == null)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, inv.GetLocation()));
                return;
            }

            var hasRetry = lambda.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Any(x => MethodName(x) == "EnableRetryOnFailure");
            if (!hasRetry)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, inv.GetLocation()));
            }
        }

        private static string? MethodName(InvocationExpressionSyntax inv)
            => (inv.Expression as MemberAccessExpressionSyntax)?.Name.Identifier.ValueText;
    }
}
