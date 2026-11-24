using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers.DomainServices
{
    /// <summary>
    /// QG-103: Notifier — декоратор бизнес-методов агрегата: реализует интерфейс агрегата
    /// (IAggregate&lt;T&gt; или конкретный I{Context}Aggregate) и реактивно рассылает результат
    /// подписчикам через IObservable&lt;AggregateResult&lt;...&gt;&gt;.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NotifierDecoratorAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "QG103";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Notifier декорирует агрегат и рассылает результат",
            "Notifier '{0}' должен реализовать интерфейс агрегата (I{Context}Aggregate или IAggregate<T>) и IObservable<AggregateResult<...>>, чтобы декорировать бизнес-методы и рассылать результат подписчикам",
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

            if (type.Name != "Notifier")
            {
                return;
            }

            var implementsAggregate = type.AllInterfaces.Any(i => i.Name.EndsWith("Aggregate"));

            var broadcastsResult = type.AllInterfaces.Any(i =>
                i.Name == "IObservable" &&
                i.TypeArguments.Length == 1 &&
                i.TypeArguments[0].Name == "AggregateResult");

            if (!implementsAggregate || !broadcastsResult)
            {
                var location = type.Locations.FirstOrDefault();
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, location, type.Name));
            }
        }
    }
}
