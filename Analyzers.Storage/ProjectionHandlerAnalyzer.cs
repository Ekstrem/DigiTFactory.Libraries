using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.Storage
{
    /// <summary>QG-304: Проекция — IDomainEventHandler&lt;TBC&gt; (+ IObserver&lt;IDomainEvent&lt;TBC&gt;&gt;).</summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ProjectionHandlerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG304";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Проекция — IDomainEventHandler",
            "Тип '{0}' (имя *EventHandler/*EventConsumer) должен реализовать IDomainEventHandler<TBC> (+ IObserver<IDomainEvent<TBC>>)",
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

            if (!type.Name.EndsWith("EventHandler"))
            {
                return;
            }

            if (!type.Implements("IDomainEventHandler"))
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
