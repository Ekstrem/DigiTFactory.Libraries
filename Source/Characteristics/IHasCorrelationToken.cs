using System;

namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Содержит маркер корреляции.
    /// </summary>
    /// <typeparam name="T">Тип маркера.</typeparam>
    public interface IHasCorrelationToken<out T>
    {
        /// <summary>
        /// Идентификатор комманды, создавшей новую версию.
        /// </summary>
        T CorrelationToken { get; }
    }
    
    /// <summary>
    /// Содержит маркер корреляции.
    /// </summary>
    public interface IHasCorrelationToken : IHasCorrelationToken<Guid> { }
}