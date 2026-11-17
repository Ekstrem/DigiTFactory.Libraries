using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>QG-206: Request DTO плоский — не повторяет доменные Value Objects.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class RequestDtoFlatAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG206";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Request DTO плоский",
            "Тип '{0}' (имя *Command/*Query) не должен реализовать IValueObject — Request DTO плоский, без доменных VO",
            "DigiTFactory",
            DiagnosticSeverity.Warning,
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

            if (!(type.Name.EndsWith("Command") || type.Name.EndsWith("Query")))
            {
                return;
            }

            if (type.Implements("IValueObject"))
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
