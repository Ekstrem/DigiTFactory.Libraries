using System;

namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Сведения о команде к агрегату.
    /// </summary>
    public interface ICommandMetadata : IHasComplexKey, ICommandSubject
    {
        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        Guid CorrelationToken { get; }

        /// <summary>
        /// Идентификатор ветви.
        /// </summary>
        Guid BranchId { get; }
    }
}