using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers
{
    /// <summary>QG-011: анемичная модель иммутабельна (readonly-поля, без публичных сеттеров).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoMutableAnemicModelAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG011";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Анемичная модель иммутабельна",
            "AnemicModel '{0}' имеет публичный setter '{1}' — анемичная модель должна быть иммутабельной (без сеттеров)",
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

            if (!type.AllInterfaces.Any(i => i.Name == "IAnemicModel"))
            {
                return;
            }

            foreach (var prop in type.GetMembers().OfType<IPropertySymbol>())
            {
                if (prop.SetMethod != null && !prop.SetMethod.IsInitOnly && prop.SetMethod.DeclaredAccessibility == Accessibility.Public)
                {
                    var location = prop.Locations.FirstOrDefault();
                    ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name, prop.Name));
                }
            }
        }
    }
}
