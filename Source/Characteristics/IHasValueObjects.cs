using System.Collections.Generic;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Имеет объект-значения.
    /// </summary>
    public interface IHasValueObjects
    {
        /// <summary>
        /// Получить объекты значения.
        /// </summary>
        /// <returns>Словарь объект-значений.</returns>
        IDictionary<string, IValueObject> GetValueObjects();
    }
}