using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Domain
{
    /// <summary>QG-014: каждый VO (по имени *ValueObject/*Value) реализует IValueObject.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ValueObjectMarkerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG014";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "VO реализует IValueObject",
            "Тип '{0}' (имя *ValueObject/*Value) должен реализовать IValueObject",
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

            if (!(type.Name.EndsWith("ValueObject") || type.Name.EndsWith("Value")))
            {
                return;
            }

            if (!type.Implements("IValueObject"))
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
