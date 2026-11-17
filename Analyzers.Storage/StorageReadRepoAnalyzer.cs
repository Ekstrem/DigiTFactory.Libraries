using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Storage
{
    /// <summary>QG-302: Read Store — библиотечный AddReadStorePostgres/IReadModelStore, не кастомный *ReadRepository.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class StorageReadRepoAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG302";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Read Store — библиотечный",
            "Тип '{0}' — кастомный read-репозиторий; используй библиотечный AddReadStorePostgres + IReadModelStore/IReadRepository",
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

            if (!type.Name.EndsWith("ReadRepository"))
            {
                return;
            }

            var location = type.Locations.FirstOrDefault();
            ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
        }
    }
}
