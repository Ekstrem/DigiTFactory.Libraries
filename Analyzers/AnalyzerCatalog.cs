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
                    new Domain.InternalImplementationAnalyzer(),
                    new Domain.ValidatorInterfaceAnalyzer(),
                    new Domain.AnemicModelFactoryAnalyzer(),
                },
                [LayerKind.DomainServices] = new DiagnosticAnalyzer[]
                {
                    new DomainServices.NoCustomInfrastructureAnalyzer(),
                    new DomainServices.ProviderNotifierDecoratorAnalyzer(),
                    new DomainServices.ProviderDefaultModelAnalyzer(),
                    new DomainServices.NotifierDecoratorAnalyzer(),
                    new DomainServices.BusAdapterAnalyzer(),
            new DomainServices.AggregateCompletionAnalyzer(),
                    new DomainServices.RegisterDependenciesAnalyzer(),
                    new DomainServices.ProviderDiAnalyzer(),
                },
                [LayerKind.Application] = new DiagnosticAnalyzer[]
                {
                    new Application.CommandInterfaceAnalyzer(),
                    new Application.ActorFromAuthAnalyzer(),
                    new Application.HandlerProviderAnalyzer(),
                    new Application.HandlerNoDomainLogicAnalyzer(),
                    new Application.CommandHandlerProviderAnalyzer(),
                    new Application.QueryHandlerReadRepoAnalyzer(),
                    new Application.RequestDtoFlatAnalyzer(),
                    new Application.AggregateProviderScopingAnalyzer(),
                    new Application.QueryRepositoryScopingAnalyzer(),
                    new Application.HandlerModelBuildingAnalyzer(),
                },
                [LayerKind.Storage] = new DiagnosticAnalyzer[]
                {
                    new Storage.StorageCommandDbAnalyzer(),
                    new Storage.StorageReadRepoAnalyzer(),
                    new Storage.ReadModelMarkerAnalyzer(),
                    new Storage.ProjectionHandlerAnalyzer(),
                    new Storage.ProjectionIdempotencyAnalyzer(),
                    new Storage.ProjectionUpsertAnalyzer(),
                    new Storage.ValueObjectSerializationAnalyzer(),
                },
                [LayerKind.CrossCutting] = new DiagnosticAnalyzer[]
                {
                    new CrossCutting.NoAbstractAggregateAnalyzer(),
                    new CrossCutting.NoNotImplementedAnalyzer(),
                    new Resilience.NoNewHttpClientAnalyzer(),
                    new Resilience.EfEnableRetryOnFailureAnalyzer(),
                    new Resilience.HttpClientResiliencePolicyAnalyzer(),
                    new Resilience.NoSyncOverAsyncAnalyzer(),
                    new Resilience.NoHandRolledRetryAnalyzer(),
                    new Resilience.HealthChecksAnalyzer(),
                    new Testing.TestNamingAnalyzer(),
                    new Testing.AggregateCoveragePresenceAnalyzer(),
                },
            };

        public static IReadOnlyList<DiagnosticAnalyzer> All =>
            ByLayer.Values.SelectMany(x => x).ToList();
    }
}
