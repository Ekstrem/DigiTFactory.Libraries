namespace Hive.SeedWorks.Business
{
    /// <summary>
    /// Объект имеет идентификационное поле.
    /// </summary>
    /// <typeparam name="TKey">Ключевой тип поиска родителя.</typeparam>
    public interface IHasKey<out TKey> : IHasKey
    {
        /// <summary>
        /// Имеет родителя.
        /// </summary>
        TKey Id { get; }
    }


    /// <summary>
    /// Объект имеет идентификационное поле.
    /// </summary>
    public interface IHasKey { }
}