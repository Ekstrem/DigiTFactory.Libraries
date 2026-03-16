using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.AbstractAggregate.Cache;
using DigiTFactory.Libraries.AbstractAggregate.DynamicTypes;
using DigiTFactory.Libraries.AbstractAggregate.Invariants;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.AbstractAggregate.Operations;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Factory
{
    /// <summary>
    /// Фабрика агрегатов, строящая Aggregate&lt;TBoundedContext&gt; из метаданных.
    /// </summary>
    public sealed class AbstractAggregateFactory<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IMetadataCache _cache;

        public AbstractAggregateFactory(IMetadataCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Создать агрегат из метаданных.
        /// </summary>
        /// <param name="aggregateName">Имя агрегата в хранилище метаданных.</param>
        /// <param name="model">Текущая анемичная модель (загруженная из Event Store).</param>
        /// <param name="ct">Токен отмены.</param>
        /// <returns>Готовый агрегат с операциями из метаданных.</returns>
        public async Task<Aggregate<TBoundedContext>> CreateAsync(
            string aggregateName,
            IAnemicModel<TBoundedContext> model,
            CancellationToken ct = default)
        {
            var metadata = await _cache.GetOrLoadAsync(aggregateName, ct);
            var scope = BuildScope(metadata);
            return Aggregate<TBoundedContext>.CreateInstance(model, scope);
        }

        /// <summary>
        /// Получить метаданные агрегата (для создания DynamicAnemicModel и т.д.).
        /// </summary>
        public async Task<AggregateMetadata> GetMetadataAsync(
            string aggregateName, CancellationToken ct = default)
        {
            return await _cache.GetOrLoadAsync(aggregateName, ct);
        }

        /// <summary>
        /// Построить IBoundedContextScope из метаданных.
        /// </summary>
        private DynamicBoundedContextScope<TBoundedContext> BuildScope(AggregateMetadata metadata)
        {
            var operations = metadata.Operations.ToDictionary(
                op => op.CommandName,
                op => (IAggregateBusinessOperation<TBoundedContext>)BuildOperation(op));

            return new DynamicBoundedContextScope<TBoundedContext>(
                operations,
                Array.Empty<IBusinessEntityValidator<TBoundedContext>>());
        }

        /// <summary>
        /// Построить MetadataBusinessOperation из метаданных операции.
        /// </summary>
        private MetadataBusinessOperation<TBoundedContext> BuildOperation(OperationMetadata operationMetadata)
        {
            var specifications = operationMetadata.Invariants
                .Select(inv => RuleExpressionParser.Parse<TBoundedContext>(inv))
                .ToList();

            return new MetadataBusinessOperation<TBoundedContext>(operationMetadata, specifications);
        }
    }
}
