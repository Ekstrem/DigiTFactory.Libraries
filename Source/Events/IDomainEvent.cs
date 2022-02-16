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
    /// <typeparam name="TKey">Ключевое поле агрегата.</typeparam>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    public interface IDomainEvent<TBoundedContext, out TKey> :
        IDomainEvent<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Экземпляр агрегата.
        /// </summary>
        TKey AggregateId { get; }
    }

    /// <summary>
    /// Доменное событие предметной области.
    /// </summary>
    public interface IDomainEvent<TBoundedContext> :
        IComplexKey,
        IBoundedContextDescription,
        IDomainOperationResult
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Имя бизнес-операции - доменное событие.
        /// </summary>
        ICommandToAggregate Command { get; }

        /// <summary>
        /// Словарь изменившихся объект значений.
        /// </summary>
        IDictionary<string, IValueObject> ChangedValueObjects { get; }
    }
}