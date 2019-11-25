using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Сущность.
    /// </summary>
    public interface IEntity : IValueObject, IHasKey { }

    /// <summary>
    /// Сущность.
    /// </summary>
    /// <typeparam name="TKey">Тип ключевого поля.</typeparam>
    public interface IEntity<out TKey> : IValueObject, IHasKey<TKey> { }
}