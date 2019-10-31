using System;
using System.Collections.Generic;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Доменное событие предметной области.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст события.</typeparam>
    public class DomainEvent<TBoundedContext> :
        IDomainEvent<TBoundedContext> 
        where TBoundedContext : IBoundedContext
    {
        private readonly IDictionary<string, IValueObject> _changedValueObjects;
        private readonly Guid _id;
        private readonly CommandToAggregate _command;
        private readonly DateTime _timeStamp;

        public DomainEvent(
            Guid id,
            int version,
            CommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects)
        {
            _id = id;
            _command = command;
            _changedValueObjects = changedValueObjects;
            _timeStamp = DateTime.Now;
        }

        /// <summary>
        /// Экземпляр агрегата.
        /// </summary>
        public Guid Id => _id;

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