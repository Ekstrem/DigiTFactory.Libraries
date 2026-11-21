using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.DomainServices
{
    /// <summary>
    /// QG-110: командный обработчик изменяет агрегат через
    /// IAggregateProviderWithCompletion.GetAggregateWithCompletionAsync + await completion
    /// (TCS-мост, долговечная доставка события), а не через голый GetAggregateAsync без ожидания доставки.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AggregateCompletionAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG110";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Команда не дожидается доставки события",
            "Используй IAggregateProviderWithCompletion.GetAggregateWithCompletionAsync + await completion вместо GetAggregateAsync (QG-110): агрегат остаётся синхронным, а команда дожидается долговечной записи события в шину",
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
            // тесты исключаем (тот же скоупинг, что QG-211/212)
            if (ctx.Node.SyntaxTree.FilePath.Contains("Tests"))
            {
                return;
            }

            var inv = (InvocationExpressionSyntax)ctx.Node;

            var name = inv.Expression switch
            {
                MemberAccessExpressionSyntax m => m.Name.Identifier.ValueText,
                _ => null,
            };

            if (name == "GetAggregateAsync")
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, inv.GetLocation()));
            }
        }
    }
}
