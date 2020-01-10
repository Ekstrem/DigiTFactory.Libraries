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
        private readonly IComplexKey _key;
        private readonly IAnemicModel<TBoundedContext> _anemicModel;
        private readonly IBoundedContextScope<TBoundedContext> _scope;
        private string _commandName;
        private string _subjectName;

        private Aggregate(
            IComplexKey key,
            IAnemicModel<TBoundedContext> anemicModel,
            IBoundedContextScope<TBoundedContext> scope)
        {
            _key = key;
            _anemicModel = anemicModel;
            _scope = scope;
        }

        /// <summary>
        /// Идентификатор агрегата.
        /// </summary>
        public Guid Id => _key.Id;

        /// <summary>
        /// Текущая версия агрегата.
        /// </summary>
        public long Version => _key.Version;

        /// <summary>
        /// Идентификатор комманды источника последней версии.
        /// </summary>
        public Guid CorrelationToken => _key.CorrelationToken;

        /// <summary>
        /// Корень модели сущности.
        /// </summary>
        public IAggregateRoot<TBoundedContext> Root => _anemicModel;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> Invariants => _anemicModel.Invariants;

        /// <summary>
        /// Бизнес-операции - фабрики.
        /// </summary>
        public IReadOnlyDictionary<string, IAggregateBusinessOperationFactory<TBoundedContext>> Operations => _scope.Operations;

        /// <summary>
        /// Валидаторы модели бизнес-объекта.
        /// </summary>
        public IReadOnlyList<IBusinessEntityValidator<TBoundedContext>> Validators => _scope.Validators;

        /// <summary>
        /// Имя метода агрегата, который вызывает команда.
        /// </summary>
        public string CommandName => _commandName;

        /// <summary>
        /// Имя субъекта бизнес-операции.
        /// </summary>
        public string SubjectName => _subjectName;

        public static IAggregate<TBoundedContext> CreateInstance(
            IComplexKey key,
            IAnemicModel<TBoundedContext> anemicModel, 
            IBoundedContextScope<TBoundedContext> scope)
            => new Aggregate<TBoundedContext>(key, anemicModel, scope);

        public static IAggregate<TBoundedContext> CreateInstance(
            IAnemicModel<TBoundedContext> anemicModel,
            IAggregate<TBoundedContext> currentAggregate,
            CommandToAggregate command)
            => new Aggregate<TBoundedContext>(
                ComplexKey.CreateWithUsingCorrelationToken(command),
                anemicModel,
                currentAggregate);
    }
}
