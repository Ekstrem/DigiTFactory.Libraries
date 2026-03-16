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
        public AggregateResult<TBoundedContext, IAnemicModel<TBoundedContext>> Handle(
            IAnemicModel<TBoundedContext> model,
            CommandToAggregate command,
            IBoundedContextScope<TBoundedContext> scope)
        {
            // Извлекаем incoming VO из команды
            // Модель должна быть DynamicAnemicModel с новыми значениями VO
            var incomingVOs = model.GetValueObjects();

            // Загружаем текущее состояние из scope (агрегат хранит старую модель)
            // В SeedWorks паттерне: model = новое состояние, а агрегат (через scope) = старое
            // Здесь model уже содержит incoming данные от вызывающего кода

            // Строим объединённую модель согласно merge strategy
            var mergedModel = ModelMerger.Merge(
                GetCurrentState(scope, model),
                incomingVOs,
                command,
                _metadata);

            // Создаём BusinessOperationData (diff старого и нового)
            var currentModel = GetCurrentState(scope, model);
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

        /// <summary>
        /// Получить текущее состояние. Если модель содержит пустую версию (Version == 0),
        /// значит это новый агрегат — используем пустую модель.
        /// </summary>
        private static IAnemicModel<TBoundedContext> GetCurrentState(
            IBoundedContextScope<TBoundedContext> scope,
            IAnemicModel<TBoundedContext> model)
        {
            // В данном контексте model уже передан как "текущее состояние" из AggregateProvider
            return model;
        }
    }
}
