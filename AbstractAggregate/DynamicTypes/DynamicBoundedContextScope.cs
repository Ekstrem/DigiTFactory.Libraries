using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.DynamicTypes
{
    /// <summary>
    /// Динамическая область ограниченного контекста, построенная из метаданных.
    /// Дополнительно хранит текущую модель агрегата (current state из Event Store),
    /// чтобы операции могли отличать current state от incoming данных.
    /// </summary>
    public sealed class DynamicBoundedContextScope<TBoundedContext> : IBoundedContextScope<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Создание scope из набора операций и валидаторов.
        /// </summary>
        public DynamicBoundedContextScope(
            IReadOnlyDictionary<string, IAggregateBusinessOperation<TBoundedContext>> operations,
            IReadOnlyList<IBusinessEntityValidator<TBoundedContext>> validators,
            IAnemicModel<TBoundedContext>? currentModel = null)
        {
            Operations = operations;
            Validators = validators;
            CurrentModel = currentModel;
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, IAggregateBusinessOperation<TBoundedContext>> Operations { get; }

        /// <inheritdoc />
        public IReadOnlyList<IBusinessEntityValidator<TBoundedContext>> Validators { get; }

        /// <summary>
        /// Текущее состояние агрегата (загруженное из Event Store).
        /// Используется MetadataBusinessOperation для разделения current vs incoming.
        /// </summary>
        public IAnemicModel<TBoundedContext>? CurrentModel { get; set; }
    }
}
