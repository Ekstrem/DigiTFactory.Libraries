using System;
using System.Collections.Generic;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Доменное событие предметной области.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст события.</typeparam>
    /// <typeparam name="TKey">Ключевое поле агрегата.</typeparam>
    public class DomainEvent<TBoundedContext, TKey> :
        IDomainEvent<TBoundedContext, TKey> 
        where TBoundedContext : IBoundedContext
    {
        private readonly IDictionary<string, IValueObject> _changedValueObjects;
        private readonly TKey _aggregateIdId;
        private readonly CommandToAggregate _command;
        private readonly DateTime _timeStamp;

        public DomainEvent(
            TKey aggregateIdId,
            CommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects)
        {
            _aggregateIdId = aggregateIdId;
            _command = command;
            _changedValueObjects = changedValueObjects;
            _timeStamp = DateTime.Now;
        }

        /// <summary>
        /// Экземпляр агрегата.
        /// </summary>
        public TKey AggregateId => _aggregateIdId;

        /// <summary>
        /// Имя бизнес-операции - доменное событие.
        /// </summary>
        public CommandToAggregate Command => _command;

        /// <summary>
        /// Словарь изменившихся объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> ChangedValueObjects => _changedValueObjects;

        /// <summary>
        /// Имя ограниченного контекста в котором произошло событие.
        /// </summary>
        public string BoundedContext => typeof(TBoundedContext).Name;

        /// <summary>
        /// Версия аггрегата.
        /// </summary>
        public DateTime TimeStamp => _timeStamp;
    }
}