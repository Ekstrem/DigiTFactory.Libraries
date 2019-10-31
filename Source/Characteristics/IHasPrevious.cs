namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Объект имеет предыдущий.
    /// </summary>
    /// <typeparam name="TKey">Ключевой тип поиска родителя.</typeparam>
    public interface IHasPrevious<TKey>
    {
        /// <summary>
        /// Имеет родителя.
        /// </summary>
        bool IsHasPrevious { get; }

        /// <summary>
        /// Идентификатор предыдущего.
        /// </summary>
        TKey PreviousId { get; set; }
    }
}