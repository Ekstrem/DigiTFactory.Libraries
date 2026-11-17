using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Domain
{
    /// <summary>QG-016: инварианты — через IBusinessOperationValidator/IBusinessOperationAssertion, не ad-hoc.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ValidatorInterfaceAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG016";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Инвариант через framework-интерфейс",
            "Тип '{0}' (имя *Validator/*Assertion) должен реализовать IBusinessOperationValidator/IBusinessOperationAssertion/IBusinessOperationSpecification",
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

            if (!(type.Name.EndsWith("Validator") || type.Name.EndsWith("Assertion")))
            {
                return;
            }

            var ok = type.Implements("IBusinessOperationValidator")
                  || type.Implements("IBusinessOperationAssertion")
                  || type.Implements("IBusinessOperationSpecification");

            if (!ok)
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
