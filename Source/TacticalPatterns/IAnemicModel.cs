using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// </summary>
    public interface IAnemicModel<TBoundedContext> :
        IComplexKey
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        IDictionary<string, IValueObject> ValueObjects { get; }
    }
}