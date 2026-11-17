using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Storage
{
    /// <summary>QG-301: Event Store — библиотечный AddEventStorePostgres/EventStoreDbContext, не кастомный CommandDbContext.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class StorageCommandDbAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG301";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Event Store — библиотечный",
            "Тип '{0}' — кастомный command DbContext; используй библиотечный EventStoreDbContext + AddEventStorePostgres (IAnemicModelRepository)",
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

            if (type.Name != "CommandDbContext")
            {
                return;
            }

            var location = type.Locations.FirstOrDefault();
            ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
        }
    }
}
