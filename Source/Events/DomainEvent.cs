using System;
using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Definition;
using Hive.SeedWorks.Result;
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
        private readonly long _version;
        private readonly ICommandToAggregate _command;
        private readonly IBoundedContextDescription _description;
        private readonly DomainOperationResultEnum _result;
        private readonly string _reason;

        public DomainEvent(
            Guid id, long version,
            ICommandToAggregate command,
            IDictionary<string, IValueObject> changedValueObjects,
            IBoundedContextDescription description,
            DomainOperationResultEnum result,
            string reason)
        {
            _id = id;
            _version = version;
            _command = command;
            _changedValueObjects = changedValueObjects;
            _description = description;
            _result = result;
            _reason = reason;
        }

        /// <summary>
        /// Экземпляр агрегата.
        /// </summary>
        public Guid Id => _id;
        
        /// <summary>
        /// Версия агрегата, над которой выполнялась команда.
        /// </summary>
        public long Version => _version;

        /// <summary>
        /// Имя бизнес-операции - доменное событие.
        /// </summary>
        public ICommandToAggregate Command => _command;

        /// <summary>
        /// Словарь изменившихся объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> ChangedValueObjects => _changedValueObjects;

        /// <summary>
        /// Имя ограниченного контекста в котором произошло событие.
        /// </summary>
        public string BoundedContext => typeof(TBoundedContext).Name;

        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        public string ContextName => _description.ContextName;
        
        /// <summary>
        /// Версия микросервиса.
        /// </summary>
        public int MicroserviceVersion => _description.MicroserviceVersion;

        /// <summary>
        /// Ресультат доменной операции.
        /// </summary>
        public DomainOperationResultEnum Result => _result;

        /// <summary>
        /// Причина ошибки при выполнении доменной операции.
        /// </summary>
        public string Reason => _reason;
    }
}