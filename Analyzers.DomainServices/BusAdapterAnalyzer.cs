using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.DomainServices
{
    /// <summary>QG-104: BusAdapter и сохраняет событие (SaveEventAsync), и публикует (IEventBus.Publish).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class BusAdapterAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG104";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "BusAdapter сохраняет и публикует",
            "BusAdapter должен и сохранять событие (SaveEventAsync), и публиковать его (IEventBus.Publish)",
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
            if (cls.Identifier.ValueText != "BusAdapter")
            {
                return;
            }

            var ids = cls.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Select(id => id.Identifier.ValueText)
                .Distinct().ToList();

            var saves = ids.Contains("SaveEventAsync");
            var publishes = ids.Contains("Publish") || ids.Contains("IEventBus");

            if (!(saves && publishes))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, cls.Identifier.GetLocation()));
            }
        }
    }
}
