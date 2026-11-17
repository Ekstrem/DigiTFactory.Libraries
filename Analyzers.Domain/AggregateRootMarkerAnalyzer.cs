using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Domain
{
    /// <summary>QG-013: корень агрегата реализует IAggregateRoot&lt;TBC&gt; (а не только IValueObject).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AggregateRootMarkerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG013";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Корень агрегата реализует IAggregateRoot",
            "Тип '{0}' (имя *Root) должен реализовать IAggregateRoot<TBC>, а не только IValueObject",
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

            if (!type.Name.EndsWith("Root"))
            {
                return;
            }

            if (!type.AllInterfaces.Any(i => i.Name == "IAggregateRoot"))
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
