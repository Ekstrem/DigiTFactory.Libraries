using System;
using System.Collections.Generic;
using Hive.SeedWorks.Business;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.LifeCircle
{
    public class Aggregate<TBoundedContext> : 
        IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IHasComplexKey _key;
        private readonly IAnemicModel<TBoundedContext> _anemicModel;
        private readonly IList<IAggregateBusinessOperationFactory<TBoundedContext>> _operations;
        private readonly IList<IBusinessValidator<TBoundedContext>> _validators;

        private Aggregate(
            IHasComplexKey key,
            IAnemicModel<TBoundedContext> anemicModel,
            IList<IAggregateBusinessOperationFactory<TBoundedContext>> operations,
            IList<IBusinessValidator<TBoundedContext>> validators)
        {
            _key = key;
            _anemicModel = anemicModel;
            _operations = operations;
            _validators = validators;
        }

        /// <summary>
        /// Идентификатор агрегата.
        /// </summary>
        public Guid Id => _key.Id;

        //Текущая версия агрегата.
        public int VersionNumber => _key.VersionNumber;

        //Дата создания последней версии.
        public DateTime Stamp => _key.Stamp;

        /// <summary>
        /// Идентификатор комманды источника последней версии.
        /// </summary>
        public Guid CommandId => _key.CommandId;

        /// <summary>
        /// Корень модели сущности.
        /// </summary>
        public IAggregateRoot<TBoundedContext> Root => _anemicModel.Root;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> ValueObjects => _anemicModel.ValueObjects;

        /// <summary>
        /// Бизнес-операции - фабрики.
        /// </summary>
        public IList<IAggregateBusinessOperationFactory<TBoundedContext>> Operations => _operations;

        /// <summary>
        /// Валидаторы модели бизнес-объекта.
        /// </summary>
        public IList<IBusinessValidator<TBoundedContext>> Validators => _validators;

        public static IAggregate<TBoundedContext> CreateInstance(
            IHasComplexKey key,
            IAnemicModel<TBoundedContext> anemicModel, 
            IList<IAggregateBusinessOperationFactory<TBoundedContext>> operations, 
            IList<IBusinessValidator<TBoundedContext>> validators)
            => new Aggregate<TBoundedContext>(key, anemicModel, operations, validators);

        public static IAggregate<TBoundedContext> CreateInstance(
            IAnemicModel<TBoundedContext> anemicModel,
            IAggregate<TBoundedContext> currentAggregate,
            CommandToAggregate command)
            => new Aggregate<TBoundedContext>(
                command.GetNextComplexKey(currentAggregate.Id, currentAggregate.VersionNumber),
                anemicModel,
                currentAggregate.Operations,
                currentAggregate.Validators);
    }
}