using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Storage
{
    /// <summary>QG-305: проекция идемпотентна — проверка Version (existing.Version &gt;= domainEvent.Version).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ProjectionIdempotencyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG305";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Проекция идемпотентна (Version)",
            "Проекция '{0}' должна проверять Version (existing.Version >= domainEvent.Version) перед Upsert — иначе повторное событие перезапишет read-модель",
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
            if (!cls.Identifier.ValueText.EndsWith("EventHandler"))
            {
                return;
            }

            var hasVersionCheck = cls.DescendantNodes().OfType<BinaryExpressionSyntax>()
                .Where(b => b.IsKind(SyntaxKind.GreaterThanOrEqualExpression)
                         || b.IsKind(SyntaxKind.GreaterThanExpression)
                         || b.IsKind(SyntaxKind.LessThanOrEqualExpression)
                         || b.IsKind(SyntaxKind.LessThanExpression))
                .Any(b => b.ToString().Contains("Version"));

            if (!hasVersionCheck)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, cls.Identifier.GetLocation(), cls.Identifier.ValueText));
            }
        }
    }
}
