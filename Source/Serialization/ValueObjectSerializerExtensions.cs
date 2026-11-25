using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>
    /// Расширения IValueObjectSerializer для обратной совместимости: чтение нового
    /// формата с fallback на легаси <c>{"$type", "value"}</c>.
    /// </summary>
    public static class ValueObjectSerializerExtensions
    {
        /// <summary>
        /// Десериализует словарь VO: сначала новый формат, при неудаче — легаси
        /// <c>{"$type", "value"}</c>. В <paramref name="configureLegacy"/> можно добавить
        /// конвертеры для VO с приватным конструктором/интерфейсом (напр. IOrgasms).
        /// </summary>
        public static IReadOnlyDictionary<string, IValueObject> DeserializeWithLegacy(
            this IValueObjectSerializer serializer,
            string payload,
            Action<JsonSerializerOptions> configureLegacy = null)
        {
            try
            {
                return serializer.Deserialize(payload);
            }
            catch (Exception)
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new LegacyValueObjectJsonConverter());
                configureLegacy?.Invoke(options);

                var dict = JsonSerializer.Deserialize<IDictionary<string, IValueObject>>(payload, options);
                return dict?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    ?? new Dictionary<string, IValueObject>();
            }
        }
    }
}
