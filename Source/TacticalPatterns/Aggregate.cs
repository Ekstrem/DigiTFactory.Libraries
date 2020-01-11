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
        private string _commandName;
        private string _subjectName;

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

        /// <summary>
        /// Текущая версия агрегата.
        /// </summary>
        public long Version => _anemicModel.Version;

        /// <summary>
        /// Идентификатор комманды источника последней версии.
        /// </summary>
        public Guid CorrelationToken => _anemicModel.CorrelationToken;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> Invariants => _anemicModel.Invariants;

        /// <summary>
        /// Бизнес-операции - фабрики.
        /// </summary>
        public IReadOnlyDictionary<string, IAggregateBusinessOperation<TBoundedContext>> Operations => _scope.Operations;

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
            IAnemicModel<TBoundedContext> anemicModel, 
            IBoundedContextScope<TBoundedContext> scope)
            => new Aggregate<TBoundedContext>(anemicModel, scope);

        public static IAggregate<TBoundedContext> CreateInstance(
            IAnemicModel<TBoundedContext> anemicModel,
            IAggregate<TBoundedContext> currentAggregate,
            CommandToAggregate command)
            => new Aggregate<TBoundedContext>(
                anemicModel,
                currentAggregate);
    }
}
