using System;
using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.TacticalPatterns
{
    public class Aggregate<TBoundedContext> :
        IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IAnemicModel<TBoundedContext> _anemicModel;
        private readonly IBoundedContextScope<TBoundedContext> _scope;

        private Aggregate(
            IAnemicModel<TBoundedContext> anemicModel,
            IBoundedContextScope<TBoundedContext> scope)
        {
            _anemicModel = anemicModel;
            _scope = scope;
        }

        /// <summary>
        /// Идентификатор агрегата.
        /// </summary>
        public Guid Id => _anemicModel.Id;

        //Текущая версия агрегата.
        public int VersionNumber => _anemicModel.VersionNumber;

        //Дата создания последней версии.
        public DateTime Stamp => _anemicModel.Stamp;

        /// <summary>
        /// Идентификатор комманды источника последней версии.
        /// </summary>
        public Guid CorrelationToken => _anemicModel.CorrelationToken;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> ValueObjects => _anemicModel.ValueObjects;

        /// <summary>
        /// Бизнес-операции - фабрики.
        /// </summary>
        public IReadOnlyDictionary<string, IAggregateBusinessOperationFactory<TBoundedContext>> Operations => _scope.Operations;

        /// <summary>
        /// Валидаторы модели бизнес-объекта.
        /// </summary>
        public IReadOnlyList<IBusinessValidator<TBoundedContext>> Validators => _scope.Validators;

        public static IAggregate<TBoundedContext> CreateInstance(
            IAnemicModel<TBoundedContext> anemicModel,
            IBoundedContextScope<TBoundedContext> scope)
            => new Aggregate<TBoundedContext>(anemicModel, scope);
    }
}
