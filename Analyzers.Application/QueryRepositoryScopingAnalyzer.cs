using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>QG-212: IQueryRepository/IReadRepository инжектится только IQueryHandler (кроме тестов).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class QueryRepositoryScopingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG212";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "IQueryRepository — только в IQueryHandler",
            "Тип '{0}' инжектит IQueryRepository/IReadRepository, но не реализует IQueryHandler — query-репозиторий доступен только запросным хендлерам",
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

            if (type.Implements("IQueryHandler"))
            {
                return;
            }

            var path = type.Locations.FirstOrDefault()?.SourceTree?.FilePath;
            if (path != null && path.Contains("Tests"))
            {
                return;
            }

            var hasQueryRepo = type.Constructors
                .Where(c => c.DeclaredAccessibility == Accessibility.Public)
                .SelectMany(c => c.Parameters)
                .Any(p => p.Type.Name == "IQueryRepository");

            if (hasQueryRepo)
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
