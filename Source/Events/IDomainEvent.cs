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
    public interface IDomainEvent<TBoundedContext> :
        IComplexKey
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Имя бизнес-операции - доменное событие.
        /// </summary>
        CommandToAggregate Command { get; }

        /// <summary>
        /// Словарь изменившихся объект значений.
        /// </summary>
        IDictionary<string, IValueObject> ChangedValueObjects { get; }

        /// <summary>
        /// Имя ограниченного контекста в котором произошло событие.
        /// </summary>
        string BoundedContext { get; }

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