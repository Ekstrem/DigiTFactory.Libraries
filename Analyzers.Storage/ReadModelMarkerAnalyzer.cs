using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Storage
{
    /// <summary>QG-303: Read-модель реализует IReadModel&lt;TBC&gt;.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ReadModelMarkerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG303";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Read-модель реализует IReadModel",
            "Тип '{0}' (имя *ReadModel) должен реализовать IReadModel<TBC>",
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

            if (!type.Name.EndsWith("ReadModel"))
            {
                return;
            }

            if (type.Implements("IReadModel"))
            {
                return;
            }

            // Top-level read-модель обязана иметь Version (идемпотентность, QG-305).
            // Вложенные объекты вида OrgasmsReadModel (без Version) — не read-модель.
            var hasVersion = type.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == "Version");
            if (!hasVersion)
            {
                return;
            }

            var location = type.Locations.FirstOrDefault();
            ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
        }
    }
}
