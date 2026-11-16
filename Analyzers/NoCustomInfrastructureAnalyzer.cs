using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers
{
    /// <summary>QG-106/QG-301: свои инфраструктурные интерфейсы/реализации запрещены — используй библиотечный DigiTFactory.</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NoCustomInfrastructureAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG106";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Своя инфраструктура запрещена",
            "Тип '{0}' — самописная инфраструктура; используй библиотечный DigiTFactory (QG-106/QG-301)",
            "DigiTFactory",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly HashSet<string> ForbiddenTypeNames = new HashSet<string>
        {
            "IRepository",
            "IReadRepository",
            "IQueryRepository",
            "ICommandRepository",
            "IEventBus",
            "IAnemicModelRepository",
            "ICommandDbContext",
            "IBusAdapter",
            "INotifier",
            "IReadModelStore"
        };

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
            if (type.TypeKind != TypeKind.Class && type.TypeKind != TypeKind.Interface)
            {
                return;
            }

            if (!ForbiddenTypeNames.Contains(type.Name))
            {
                return;
            }

            var assemblyName = type.ContainingAssembly?.Name ?? string.Empty;
            if (assemblyName.StartsWith("DigiTFactory.Libraries"))
            {
                return; // сам фреймворк — не нарушение
            }

            var location = type.Locations.FirstOrDefault();
            ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
        }
    }
}
