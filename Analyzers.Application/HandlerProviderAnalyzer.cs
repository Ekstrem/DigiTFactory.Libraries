using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>QG-202: обработчик использует провайдер, а не Aggregate.CreateInstance напрямую.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class HandlerProviderAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG202";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Обработчик использует провайдер",
            "Обработчик '{0}' вызывает Aggregate.CreateInstance напрямую — используй AggregateProvider (декорирует агрегат Notifier'ом, QG-101)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeClass(SyntaxNodeAnalysisContext ctx)
        {
            var cls = (ClassDeclarationSyntax)ctx.Node;
            if (!cls.Identifier.ValueText.EndsWith("Handler"))
            {
                return;
            }

            var hasDirectFactory = cls.DescendantNodes().OfType<MemberAccessExpressionSyntax>()
                .Any(ma => ma.Name.Identifier.ValueText == "CreateInstance"
                        && ma.Expression.ToString() == "Aggregate");

            if (hasDirectFactory)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, cls.Identifier.GetLocation(), cls.Identifier.ValueText));
            }
        }
    }
}
