namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Обёртка над простыми системными типами.
    /// </summary>
    public class ValueObjectWrapper<T> : IValueObject
    {
        private readonly T _value;

        internal ValueObjectWrapper(T value) => _value = value;

        /// <summary>
        /// Значение которое необхидимо предоставить как объект-значение.
        /// </summary>
        public T Value => _value;
    }

    public static class ValueObjectWrapperExtention
    {
        /// <summary>
        /// Преобразовать к объекту значению.
        /// </summary>
        /// <param name="value">
        /// Простой тип, который нужно преобразовать к <see cref="IValueObject"/>.
        /// </param>
        /// <typeparam name="T">Тип значения.</typeparam>
        /// <returns><see cref="IValueObject"/></returns>
        public static IValueObject ToValueObject<T>(this T value)
        => new ValueObjectWrapper<T>(value);
    }
}