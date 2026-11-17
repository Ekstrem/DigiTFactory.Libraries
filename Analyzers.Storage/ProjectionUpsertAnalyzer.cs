using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Storage
{
    /// <summary>QG-306: проекция пишет через IReadModelStore.UpsertAsync (читает IReadRepository.GetByIdAsync).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ProjectionUpsertAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG306";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Проекция пишет через IReadModelStore.UpsertAsync",
            "Проекция '{0}' должна писать через IReadModelStore.UpsertAsync (читает IReadRepository.GetByIdAsync), а не в обход платформенного read-стора",
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

            var ids = cls.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Select(id => id.Identifier.ValueText)
                .Distinct().ToList();

            if (!ids.Contains("UpsertAsync"))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, cls.Identifier.GetLocation(), cls.Identifier.ValueText));
            }
        }
    }
}
