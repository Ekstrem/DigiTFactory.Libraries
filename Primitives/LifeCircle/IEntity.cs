using Hive.SeedWorks.Business;

namespace Hive.SeedWorks.LifeCircle
{
    /// <summary>
    /// Сущность.
    /// </summary>
    public interface IEntity : IValueObject, IHasKey { }

    /// <summary>
    /// Сущность.
    /// </summary>
    /// <typeparam name="TKey">Тип ключевого поля.</typeparam>
    public interface IEntity<out TKey> : IEntity, IHasKey<TKey> { }
}