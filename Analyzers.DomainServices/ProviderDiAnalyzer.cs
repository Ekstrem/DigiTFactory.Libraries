using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.DomainServices
{
    /// <summary>QG-109: конструктор AggregateProvider инжектит только библиотечные зависимости (IAnemicModelRepository/IEventStoreRepository, IEventBus), не кастомные.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ProviderDiAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG109";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Конструктор провайдера — только библиотечные зависимости",
            "AggregateProvider должен инжектить IAnemicModelRepository/IEventStoreRepository (библиотечные) через конструктор, а не кастомные CommandDbContext/*Repository",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
        }

        private static void AnalyzeNamedType(SymbolAnalysisContext ctx)
        {
            var type = (INamedTypeSymbol)ctx.Symbol;
            if (type.TypeKind != TypeKind.Class)
            {
                return;
            }

            if (type.Name != "AggregateProvider")
            {
                return;
            }

            var ctorParams = type.Constructors
                .Where(c => c.DeclaredAccessibility == Accessibility.Public)
                .SelectMany(c => c.Parameters)
                .ToList();

            var hasLibraryRepo = ctorParams.Any(p =>
                p.Type.Name == "IAnemicModelRepository" || p.Type.Name == "IEventStoreRepository");

            var hasCustom = ctorParams.Any(p =>
                p.Type.Name == "CommandDbContext"
                || p.Type.Name.EndsWith("CommandRepository")
                || p.Type.Name.EndsWith("ReadRepository"));

            if (!hasLibraryRepo || hasCustom)
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
