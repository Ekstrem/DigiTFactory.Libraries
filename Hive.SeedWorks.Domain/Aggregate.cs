using System;
using System.Collections.Generic;
using Hive.SeedWorks.Business;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.Domain
{
    public class Aggregate<TBoundedContext, TKey> :
        IAggregate<TBoundedContext, TKey>
        where TBoundedContext : IBoundedContext
    {
        private readonly TKey _id;
        private readonly IHasVersion _versionInfo;
        private readonly IAnemicModel<TBoundedContext, TKey> _anemicModel;
        private readonly IList<IAggregateBusinessOperationFactory<IAnemicModel<TBoundedContext>, TBoundedContext>> _operations;
        private readonly IList<IBusinessValidator<TBoundedContext>> _validators;
        private IHasVersion _version;

        public Aggregate(
            TKey id,
            IHasVersion versionInfo,
            IAnemicModel<TBoundedContext, TKey> anemicModel,
            IList<IAggregateBusinessOperationFactory<IAnemicModel<TBoundedContext>, TBoundedContext>> operations,
            IList<IBusinessValidator<TBoundedContext>> validators)
        {
            _id = id;
            _versionInfo = versionInfo;
            _anemicModel = anemicModel;
            _operations = operations;
            _validators = validators;
        }

        /// <summary>
        /// Номер версии.
        /// </summary>
        public int Version => _versionInfo.Version;

        /// <summary>
        /// Дата создания версии.
        /// </summary>
        public DateTime Stamp => _versionInfo.Stamp;

        /// <summary>
        /// Идентификатор комманды, создавшей новую версию.
        /// </summary>
        public Guid CommandId => _versionInfo.CommandId;

        /// <summary>
        /// Имеет идентификатор.
        /// </summary>
        public TKey Id => _id;

        /// <summary>
        /// Корень модели сущности.
        /// </summary>
        public IAggregateRoot<TBoundedContext, TKey> Root => _anemicModel.Root;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> ValueObjects => _anemicModel.ValueObjects;

        /// <summary>
        /// Бизнес-операции - фабрики.
        /// </summary>
        public IList<IAggregateBusinessOperationFactory<IAnemicModel<TBoundedContext>, TBoundedContext>> Operations => _operations;

        public IList<IBusinessValidator<TBoundedContext>> Validators => _validators;

        IHasVersion IAggregate<TBoundedContext, TKey>.Version => _version;
    }
}