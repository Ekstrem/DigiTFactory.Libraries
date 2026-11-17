using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Domain
{
    /// <summary>QG-010: I{Context}AnemicModel : IAnemicModel&lt;{Context}&gt;.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AnemicModelMarkerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG010";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Анемичная модель наследует IAnemicModel",
            "Интерфейс '{0}' (имя *AnemicModel) должен наследовать IAnemicModel<TBC>",
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
            if (type.TypeKind != TypeKind.Interface)
            {
                return;
            }

            if (!type.Name.EndsWith("AnemicModel"))
            {
                return;
            }

            if (!type.AllInterfaces.Any(i => i.Name == "IAnemicModel"))
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
