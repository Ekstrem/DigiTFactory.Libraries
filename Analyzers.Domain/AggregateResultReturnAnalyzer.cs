using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Domain
{
    /// <summary>QG-007: бизнес-метод агрегата возвращает AggregateResult&lt;TBC, TAM&gt;.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AggregateResultReturnAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG007";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Бизнес-метод возвращает AggregateResult",
            "Метод '{0}.{1}' не возвращает AggregateResult — бизнес-метод агрегата должен возвращать AggregateResult<TBC, TAM>",
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

            if (!type.Implements("IAggregate"))
            {
                return;
            }

            foreach (var method in type.GetMembers().OfType<IMethodSymbol>())
            {
                if (method.MethodKind != MethodKind.Ordinary) continue;
                if (method.IsStatic) continue;
                if (method.DeclaredAccessibility != Accessibility.Public) continue;
                if (method.Name == "GetValueObjects") continue; // контракт IHasValueObjects

                if (!method.ReturnType.Name.StartsWith("AggregateResult"))
                {
                    var location = method.Locations.FirstOrDefault();
                    ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name, method.Name));
                }
            }
        }
    }
}
