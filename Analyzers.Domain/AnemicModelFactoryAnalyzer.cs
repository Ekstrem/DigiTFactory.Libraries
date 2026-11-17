using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Domain
{
    /// <summary>QG-012: AnemicModel/DefaultAnemicModel должна иметь фабрику Create(Guid id) — начальное (пустое) состояние агрегата с явным Id.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AnemicModelFactoryAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG012";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Начальное состояние через фабрику Create(Guid)",
            "Тип '{0}' должен иметь статическую фабрику Create/CreateInstance(Guid id) для начального (пустого) состояния агрегата",
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

            if (!(type.Name == "AnemicModel" || type.Name == "DefaultAnemicModel"))
            {
                return;
            }

            var hasCreate = type.GetMembers().OfType<IMethodSymbol>().Any(m =>
                m.IsStatic
                && m.DeclaredAccessibility == Accessibility.Public
                && (m.Name == "Create" || m.Name == "CreateInstance")
                && m.Parameters.Length > 0
                && m.Parameters[0].Type.Name == "Guid");

            if (!hasCreate)
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
