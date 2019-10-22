using System;

namespace Hive.SeedWorks.Business
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
        T Version { get; }

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
