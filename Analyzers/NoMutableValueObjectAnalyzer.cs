using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers
{
    /// <summary>QG-015: Value Object иммутабелен — приватный конструктор + фабрика, без публичных сеттеров.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoMutableValueObjectAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG015";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Value Object иммутабелен",
            "VO '{0}' имеет публичный setter '{1}' — Value Object должен быть иммутабельным (без сеттеров)",
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

            if (!type.AllInterfaces.Any(i => i.Name == "IValueObject"))
            {
                return;
            }

            foreach (var prop in type.GetMembers().OfType<IPropertySymbol>())
            {
                if (prop.SetMethod != null && prop.SetMethod.DeclaredAccessibility == Accessibility.Public)
                {
                    var location = prop.Locations.FirstOrDefault();
                    ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name, prop.Name));
                }
            }
        }
    }
}
