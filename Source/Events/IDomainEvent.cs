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
    public interface IDomainEvent : IHasComplexKey
    {
        /// <summary>
        /// Имя бизнес-операции - доменное событие.
        /// </summary>
        ICommandMetadata Command { get; }

        /// <summary>
        /// Словарь изменившихся объект значений.
        /// </summary>
        IDictionary<string, IValueObject> ChangedValueObjects { get; }

        /// <summary>
        /// Имя ограниченного контекста в котором произошло событие.
        /// </summary>
        string BoundedContext { get; }

        /// <summary>
        /// Идентификатор экземпляра.
        /// </summary>
        Guid InstanceId { get; }

        /// <summary>
        /// Мажорная версия микросервиса.
        /// </summary>
        byte MicroserviceMajorVersion { get; }

        /// <summary>
        /// Минорная версия микросервиса.
        /// </summary>
        byte MicroserviceMinorVersion { get; }

        /// <summary>
        /// Результат выполнения операции.
        /// </summary>
        DomainOperationResult Result { get; }
        
        /// <summary>
        /// Причина в случае неуспеха выполнения операции.
        /// </summary>
        string Reason { get; }
    }
}