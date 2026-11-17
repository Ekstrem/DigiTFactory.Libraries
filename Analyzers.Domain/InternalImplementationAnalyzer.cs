using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Domain
{
    /// <summary>QG-020: реализация модели — internal (наружу только абстракции).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class InternalImplementationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG020";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Реализация модели internal",
            "Тип '{0}' публичен, но лежит в Implementation — реализация должна быть internal (наружу только абстракции)",
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

            if (type.DeclaredAccessibility != Accessibility.Public)
            {
                return;
            }

            if (!type.ContainingNamespace.ToDisplayString().Contains("Implementation"))
            {
                return;
            }

            var location = type.Locations.FirstOrDefault();
            ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
        }
    }
}
