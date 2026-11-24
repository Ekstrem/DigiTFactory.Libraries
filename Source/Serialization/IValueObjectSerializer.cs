using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>
    /// Типизированная сериализация словаря изменённых Value Objects.
    /// Единый порт для JSON-envelope, ProtoBuf и MessagePack; формат выбирается фабрикой.
    /// </summary>
    public interface IValueObjectSerializer
    {
        /// <summary>Сериализует словарь VO по runtime-типу (не по интерфейсу IValueObject).</summary>
        string Serialize(IReadOnlyDictionary<string, IValueObject> valueObjects);

        /// <summary>Восстанавливает словарь VO по runtime-типу.</summary>
        IReadOnlyDictionary<string, IValueObject> Deserialize(string payload);
    }
}
