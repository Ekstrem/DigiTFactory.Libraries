using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>QG-211: IAggregateProvider инжектится только ICommandHandler (кроме тестов).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AggregateProviderScopingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG211";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "IAggregateProvider — только в ICommandHandler",
            "Тип '{0}' инжектит IAggregateProvider, но не реализует ICommandHandler — провайдер агрегата доступен только командным хендлерам",
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

            if (type.Implements("ICommandHandler"))
            {
                return;
            }

            var path = type.Locations.FirstOrDefault()?.SourceTree?.FilePath;
            if (path != null && path.Contains("Tests"))
            {
                return;
            }

            var hasProvider = type.Constructors
                .Where(c => c.DeclaredAccessibility == Accessibility.Public)
                .SelectMany(c => c.Parameters)
                .Any(p => p.Type.Name == "IAggregateProvider");

            if (hasProvider)
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
