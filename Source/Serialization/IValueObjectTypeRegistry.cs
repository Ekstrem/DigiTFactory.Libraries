using System;

namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>Реестр полиморфных типов VO: стабильный дискриминатор типа для сериализации.</summary>
    public interface IValueObjectTypeRegistry
    {
        /// <summary>Возвращает дискриминатор для типа (по умолчанию AssemblyQualifiedName).</summary>
        string GetTypeName(Type type);

        /// <summary>Разрешает дискриминатор в тип. Бросает при неизвестном типе.</summary>
        Type ResolveType(string typeName);
    }
}
