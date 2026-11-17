using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.DomainServices
{
    /// <summary>QG-102: при промахе провайдер собирает агрегат из DefaultAnemicModel (пустой модели).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ProviderDefaultModelAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG102";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Промах репозитория → DefaultAnemicModel",
            "AggregateProvider должен при промахе репозитория собирать агрегат из DefaultAnemicModel.Create (?? DefaultAnemicModel.Create)",
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
            if (cls.Identifier.ValueText != "AggregateProvider")
            {
                return;
            }

            var hasDefault = cls.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Any(id => id.Identifier.ValueText is "DefaultAnemicModel");

            if (!hasDefault)
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, cls.Identifier.GetLocation()));
            }
        }
    }
}
