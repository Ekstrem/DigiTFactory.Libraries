using System;
using System.Collections.Generic;
using System.Linq;
using DigiTFactory.Libraries.AbstractAggregate.DynamicTypes;
using DigiTFactory.Libraries.AbstractAggregate.Invariants;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.Operations
{
    /// <summary>
    /// Динамическая бизнес-операция, построенная из метаданных.
    /// Реализует IAggregateBusinessOperation для работы с SeedWorks Aggregate.
    /// </summary>
    public sealed class MetadataBusinessOperation<TBoundedContext> :
        IAggregateBusinessOperation<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly OperationMetadata _metadata;
        private readonly IReadOnlyList<IBusinessOperationSpecification<TBoundedContext, IAnemicModel<TBoundedContext>>> _specifications;

        /// <summary>
        /// Создание бизнес-операции из метаданных и спецификаций.
        /// </summary>
        public MetadataBusinessOperation(
            OperationMetadata metadata,
            IReadOnlyList<IBusinessOperationSpecification<TBoundedContext, IAnemicModel<TBoundedContext>>> specifications)
        {
            _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            _specifications = specifications ?? [];
        }

        /// <summary>
        /// Имя команды.
        /// </summary>
        public string CommandName => _metadata.CommandName;

        /// <inheritdoc />
        /// <remarks>
        /// model — incoming данные (новые VO от команды).
        /// Текущее состояние агрегата берётся из scope.CurrentModel (DynamicBoundedContextScope).
        /// Если CurrentModel не задан (legacy), model используется как current state.
        /// </remarks>
        public AggregateResult<TBoundedContext, IAnemicModel<TBoundedContext>> Handle(
            IAnemicModel<TBoundedContext> model,
            CommandToAggregate command,
            IBoundedContextScope<TBoundedContext> scope)
        {
            // Текущее состояние из scope (если задано), иначе fallback на model
            var currentModel = (scope is DynamicBoundedContextScope<TBoundedContext> dynamicScope
                && dynamicScope.CurrentModel != null)
                ? dynamicScope.CurrentModel
                : model;

            // Incoming VO — из переданной модели
            var incomingVOs = model.GetValueObjects();

            // Строим объединённую модель согласно merge strategy
            var mergedModel = ModelMerger.Merge(
                currentModel,
                incomingVOs,
                command,
                _metadata);

            // Создаём BusinessOperationData (diff старого и нового)
            var opData = BusinessOperationData<TBoundedContext, IAnemicModel<TBoundedContext>>
                .Commit<TBoundedContext, IAnemicModel<TBoundedContext>>(currentModel, mergedModel);

            // Валидация через спецификации из метаданных
            if (_specifications.Count > 0)
            {
                return opData.ValidateCommand(_specifications.ToArray());
            }

            // Без спецификаций — успех
            return new AggregateResultSuccess<TBoundedContext, IAnemicModel<TBoundedContext>>(opData);
        }
    }
}
