namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Объект имеет родителя.
    /// </summary>
    /// <typeparam name="TKey">Ключевой тип поиска родителя.</typeparam>
    public interface IHasParent<TKey>
    {
        /// <summary>
        /// Имеет родителя.
        /// </summary>
        bool IsHasParent { get; }

        /// <summary>
        /// Идентификатор родителя.
        /// </summary>
        TKey ParentId { get; set; }
    }
}
