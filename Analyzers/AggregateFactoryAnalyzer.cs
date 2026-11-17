using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers
{
    /// <summary>QG-005: агрегат — приватный конструктор + статическая фабрика Create/CreateInstance.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AggregateFactoryAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG005";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Агрегат: приватный конструктор + статическая фабрика",
            "Агрегат '{0}' должен иметь приватный конструктор и статическую фабрику Create/CreateInstance",
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

            if (!type.AllInterfaces.Any(i => i.Name == "IAggregate"))
            {
                return;
            }

            var hasNonPublicCtor = type.InstanceConstructors.Any(
                c => c.DeclaredAccessibility != Accessibility.Public
                     && c.DeclaredAccessibility != Accessibility.Protected
                     && c.DeclaredAccessibility != Accessibility.ProtectedOrInternal);

            var hasFactory = type.GetMembers().OfType<IMethodSymbol>().Any(
                m => m.IsStatic
                     && m.DeclaredAccessibility == Accessibility.Public
                     && (m.Name == "CreateInstance" || m.Name == "Create"));

            if (!hasNonPublicCtor || !hasFactory)
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
