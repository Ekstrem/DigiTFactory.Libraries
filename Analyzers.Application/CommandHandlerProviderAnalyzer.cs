using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Application
{
    /// <summary>QG-210: CommandHandler инжектит IAggregateProvider; исключение — [CommandHandlerNoAggregate] (warning).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class CommandHandlerProviderAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG210";
        public const string DiagnosticIdWarning = "QG210W";

        private static readonly DiagnosticDescriptor RuleError = new DiagnosticDescriptor(
            DiagnosticId,
            "CommandHandler инжектит IAggregateProvider",
            "CommandHandler '{0}' должен инжектить IAggregateProvider в конструктор (или пометить [CommandHandlerNoAggregate])",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor RuleWarning = new DiagnosticDescriptor(
            DiagnosticIdWarning,
            "CommandHandler без агрегата",
            "CommandHandler '{0}' помечен [CommandHandlerNoAggregate] — без IAggregateProvider (осознанное исключение)",
            "DigiTFactory",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(RuleError, RuleWarning);

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

            if (!type.Name.EndsWith("CommandHandler"))
            {
                return;
            }

            var hasProvider = type.Constructors
                .Where(c => c.DeclaredAccessibility == Accessibility.Public)
                .SelectMany(c => c.Parameters)
                .Any(p => p.Type.Name == "IAggregateProvider");

            if (hasProvider)
            {
                return;
            }

            var location = type.Locations.FirstOrDefault();
            var hasAttribute = type.GetAttributes().Any(a => a.AttributeClass?.Name == "CommandHandlerNoAggregate");
            var diagnostic = hasAttribute ? RuleWarning : RuleError;
            ctx.ReportDiagnostic(Diagnostic.Create(diagnostic, location, type.Name));
        }
    }
}
