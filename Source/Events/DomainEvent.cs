using System;
using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Доменное событие предметной области.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст события.</typeparam>
    public class DomainEvent : IDomainEvent
    {
        private readonly IDictionary<string, IValueObject> _changedValueObjects;
        private readonly string _boundedContextName;
        private readonly IHasComplexKey _key; 
        private readonly ICommandMetadata _command;
        private DomainOperationResult _result;
        private string _reason;
        private Guid _instanceId;
        private byte _microserviceMajorVersion;
        private byte _microserviceMinorVersion;

        public DomainEvent(
            string boundedContextName,
            IHasComplexKey key,
            ICommandMetadata command,
            IDictionary<string, IValueObject> changedValueObjects,
            DomainOperationResult result,
            string reason)
        {
            _boundedContextName = boundedContextName;
            _key = key;
            _command = command;
            _changedValueObjects = changedValueObjects;
        }

        /// <summary>
        /// Экземпляр агрегата.
        /// </summary>
        public Guid Id => _key.Id;
        
        public long Version => _key.Version;

        /// <summary>
        /// Имя бизнес-операции - доменное событие.
        /// </summary>
        public ICommandMetadata Command => _command;

        /// <summary>
        /// Словарь изменившихся объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> ChangedValueObjects => _changedValueObjects;

        /// <summary>
        /// Имя ограниченного контекста в котором произошло событие.
        /// </summary>
        public string BoundedContext => _boundedContextName;

        public Guid InstanceId => _instanceId;

        public byte MicroserviceMajorVersion => _microserviceMajorVersion;

        public byte MicroserviceMinorVersion => _microserviceMinorVersion;

        public DomainOperationResult Result => _result;

        public string Reason => _reason;
    }
}