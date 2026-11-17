using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DigiTFactory.Libraries.Analyzers
{
    /// <summary>Слои микросервиса — для группировки анализаторов и фильтрации прогона.</summary>
    public enum LayerKind
    {
        Domain,
        DomainServices,
        Application,
        Storage,
        Api,
        InternalContracts,
        CrossCutting,
    }

    /// <summary>
    /// Каркас: реестр всех анализаторов по слоям. Единственное место, где перечислены правила.
    /// DtfScan и будущие per-layer пакеты берут правила отсюда; слой выбирается фильтром `--layer`.
    /// </summary>
    public static class AnalyzerCatalog
    {
        public static readonly IReadOnlyDictionary<LayerKind, IReadOnlyList<DiagnosticAnalyzer>> ByLayer =
            new Dictionary<LayerKind, IReadOnlyList<DiagnosticAnalyzer>>
            {
                [LayerKind.Domain] = new DiagnosticAnalyzer[]
                {
                    new Domain.AggregateFactoryAnalyzer(),
                    new Domain.NoPublicSetterAnalyzer(),
                    new Domain.NoMutableAnemicModelAnalyzer(),
                    new Domain.NoMutableValueObjectAnalyzer(),
                    new Domain.ValueObjectMarkerAnalyzer(),
                    new Domain.AggregateResultReturnAnalyzer(),
                    new Domain.AnemicModelMarkerAnalyzer(),
                    new Domain.AggregateRootMarkerAnalyzer(),
                },
                [LayerKind.DomainServices] = new DiagnosticAnalyzer[]
                {
                    new DomainServices.NoCustomInfrastructureAnalyzer(),
                },
                [LayerKind.CrossCutting] = new DiagnosticAnalyzer[]
                {
                    new CrossCutting.NoAbstractAggregateAnalyzer(),
                    new CrossCutting.NoNotImplementedAnalyzer(),
                },
            };

        public static IReadOnlyList<DiagnosticAnalyzer> All =>
            ByLayer.Values.SelectMany(x => x).ToList();
    }
}
