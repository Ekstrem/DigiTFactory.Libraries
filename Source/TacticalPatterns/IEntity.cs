using DigiTFactory.Libraries.SeedWorks.Characteristics;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
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