using System;
using System.Collections.Generic;

namespace DigiTFactory.Libraries.AbstractAggregate.Serialization
{
    /// <summary>
    /// Маппинг строковых имён типов из метаданных в CLR-типы.
    /// </summary>
    public static class TypeResolver
    {
        private static readonly Dictionary<string, Type> KnownTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            ["string"] = typeof(string),
            ["String"] = typeof(string),
            ["int"] = typeof(int),
            ["Int32"] = typeof(int),
            ["long"] = typeof(long),
            ["Int64"] = typeof(long),
            ["bool"] = typeof(bool),
            ["Boolean"] = typeof(bool),
            ["double"] = typeof(double),
            ["Double"] = typeof(double),
            ["float"] = typeof(float),
            ["Single"] = typeof(float),
            ["decimal"] = typeof(decimal),
            ["Decimal"] = typeof(decimal),
            ["byte"] = typeof(byte),
            ["Byte"] = typeof(byte),
            ["short"] = typeof(short),
            ["Int16"] = typeof(short),
            ["Guid"] = typeof(Guid),
            ["DateTime"] = typeof(DateTime),
            ["DateTimeOffset"] = typeof(DateTimeOffset),
            ["TimeSpan"] = typeof(TimeSpan)
        };

        /// <summary>
        /// Разрешить CLR-тип по строковому имени.
        /// </summary>
        /// <param name="typeName">Имя типа из метаданных.</param>
        /// <returns>CLR-тип или null, если тип неизвестен.</returns>
        public static Type? Resolve(string typeName)
        {
            if (KnownTypes.TryGetValue(typeName, out var type))
                return type;

            // Попытка загрузить как FQN (например, для enum-типов)
            return Type.GetType(typeName);
        }

        /// <summary>
        /// Преобразовать строковое значение в объект указанного типа.
        /// </summary>
        public static object? ConvertValue(string? value, string typeName)
        {
            if (value == null) return null;

            var type = Resolve(typeName);
            if (type == null) return value;

            return ConvertToType(value, type);
        }

        /// <summary>
        /// Преобразовать строковое значение в указанный CLR-тип.
        /// </summary>
        public static object? ConvertToType(string value, Type targetType)
        {
            if (targetType == typeof(string)) return value;
            if (targetType == typeof(Guid)) return Guid.Parse(value);
            if (targetType == typeof(DateTime)) return DateTime.Parse(value);
            if (targetType == typeof(DateTimeOffset)) return DateTimeOffset.Parse(value);
            if (targetType == typeof(TimeSpan)) return TimeSpan.Parse(value);
            if (targetType.IsEnum) return Enum.Parse(targetType, value, ignoreCase: true);

            return Convert.ChangeType(value, targetType);
        }
    }
}
