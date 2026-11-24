using System;
using System.Collections.Generic;

namespace DigiTFactory.Libraries.SeedWorks.Serialization
{
    /// <summary>
    /// Реестр типов VO. Разрешает по зарегистрированному алиасу, затем по AssemblyQualifiedName.
    /// Регистрируй типы явно, чтобы контролировать стабильные дискриминаторы и сузить поверхность десериализации.
    /// </summary>
    public sealed class ValueObjectTypeRegistry : IValueObjectTypeRegistry
    {
        private readonly Dictionary<Type, string> _byType = new();
        private readonly Dictionary<string, Type> _byName = new(StringComparer.Ordinal);

        /// <summary>Регистрирует тип VO под явным алиасом (или AssemblyQualifiedName).</summary>
        public ValueObjectTypeRegistry Register<T>(string alias = null)
        {
            var type = typeof(T);
            var name = alias ?? type.AssemblyQualifiedName ?? type.FullName!;
            _byType[type] = name;
            _byName[name] = type;
            return this;
        }

        /// <inheritdoc />
        public string GetTypeName(Type type)
            => _byType.TryGetValue(type, out var name)
                ? name
                : (type.AssemblyQualifiedName ?? type.FullName!);

        /// <inheritdoc />
        public Type ResolveType(string typeName)
        {
            if (_byName.TryGetValue(typeName, out var registered))
            {
                return registered;
            }

            var resolved = Type.GetType(typeName);
            if (resolved == null)
            {
                throw new InvalidOperationException($"Unknown value object type discriminator: '{typeName}'. Register it in IValueObjectTypeRegistry.");
            }

            return resolved;
        }
    }
}
