namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Объект имеет следующего.
    /// </summary>
    /// <typeparam name="TKey">Ключевой тип поиска родителя.</typeparam>
    public interface IHasNext<TKey>
    {
        /// <summary>
        /// Имеет родителя.
        /// </summary>
        bool IsHasNext { get; }

        /// <summary>
        /// Идентификатор следующего.
        /// </summary>
        TKey NextId { get; set; }
    }
}