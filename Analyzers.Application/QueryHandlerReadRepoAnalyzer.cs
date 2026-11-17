using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>QG-204: IQueryHandler читает только IReadRepository/IQueryableReadRepository, не кастомный I{Context}QueryRepository / Event Store.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class QueryHandlerReadRepoAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG204";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "IQueryHandler читает только Read-репозиторий",
            "IQueryHandler '{0}' должен читать через IReadRepository/IQueryableReadRepository, а не кастомный I{Context}QueryRepository/Event Store",
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

            if (!type.Implements("IQueryHandler"))
            {
                return;
            }

            var hasWrongRead = type.Constructors
                .Where(c => c.DeclaredAccessibility == Accessibility.Public)
                .SelectMany(c => c.Parameters)
                .Any(p => p.Type.Name.EndsWith("QueryRepository")
                       || p.Type.Name == "IAnemicModelRepository"
                       || p.Type.Name == "IEventStoreRepository");

            if (hasWrongRead)
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
