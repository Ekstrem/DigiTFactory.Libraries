using System;
using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks
{
    /// <summary>
    /// Сведения о команде к агрегату.
    /// </summary>
    public interface ICommandMetadata : IHasVersion, ICommandSubject
    {
        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        Guid CorrelationToken { get; }

        /// <summary>
        /// Определяет версию. Ожидаемое использование - дата создания версии в милисекундах.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        long Version { get; }

        /// <summary>
        /// Имя метода агрегата, который вызывает команда.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// Имя субъекта бизнес-операции.
        /// Мапится на NameLastModification.
        /// </summary>
        string SubjectName { get; }
    }
}