using System;

namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Объект имеет идентификационное поле.
    /// </summary>
    /// <typeparam name="TKey">Ключевой тип поиска родителя.</typeparam>
    public interface IHasKey<out TKey>
    {
        /// <summary>
        /// Имеет идентификатор.
        /// </summary>
        TKey Id { get; }
    }


    /// <summary>
    /// Объект имеет идентификационное поле.
    /// </summary>
    public interface IHasKey : IHasKey<Guid> { }
}
