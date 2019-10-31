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
        /// Номер версии.
        /// </summary>
        T VersionNumber { get; }

        /// <summary>
        /// Дата создания версии.
        /// </summary>
        DateTime Stamp { get; }

        /// <summary>
        /// Идентификатор комманды, создавшей новую версию.
        /// </summary>
        Guid CommandId { get; }
    }

    /// <summary>
    /// Указывает на то что объект имеет версионность.
    /// </summary>
    public interface IHasVersion : IHasVersion<int> { }
}
