using System;

namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Указывает на то что объект имеет версионность.
    /// </summary>
    /// <typeparam name="T">Тип данных которым версионируется объект.</typeparam>
    public interface IHasVersion<out T>
    {
        /// <summary>
        /// Определяет версию. Ожидаемое использование - дата создания версии в милисекундах.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        T Version { get; }
    }

    /// <summary>
    /// Указывает на то что объект имеет версионность.
    /// </summary>
    public interface IHasVersion : IHasVersion<long> { }
}
