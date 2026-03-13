using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.Characteristics
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