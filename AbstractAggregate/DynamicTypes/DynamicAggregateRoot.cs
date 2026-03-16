using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.DynamicTypes
{
    /// <summary>
    /// Динамический корень агрегата, описанный метаданными.
    /// Реализует IAggregateRoot&lt;TBoundedContext&gt;.
    /// </summary>
    public sealed class DynamicAggregateRoot<TBoundedContext> : IAggregateRoot<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IReadOnlyDictionary<string, object?> _properties;

        /// <summary>
        /// Создание AggregateRoot из типового имени и набора свойств.
        /// </summary>
        public DynamicAggregateRoot(string typeName, IDictionary<string, object?> properties)
        {
            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            _properties = new ReadOnlyDictionary<string, object?>(
                new Dictionary<string, object?>(properties));
        }

        /// <summary>
        /// Имя типа.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Все свойства.
        /// </summary>
        public IReadOnlyDictionary<string, object?> Properties => _properties;

        /// <summary>
        /// Получить значение свойства с приведением типа.
        /// </summary>
        public T? Get<T>(string propertyName)
        {
            if (_properties.TryGetValue(propertyName, out var val) && val is T typed)
                return typed;
            return default;
        }

        /// <summary>
        /// Получить значение свойства без типизации.
        /// </summary>
        public object? Get(string propertyName)
        {
            return _properties.TryGetValue(propertyName, out var val) ? val : null;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not DynamicAggregateRoot<TBoundedContext> other) return false;
            if (TypeName != other.TypeName) return false;
            if (_properties.Count != other._properties.Count) return false;

            foreach (var (key, value) in _properties)
            {
                if (!other._properties.TryGetValue(key, out var otherValue)) return false;
                if (!Equals(value, otherValue)) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(TypeName);
            foreach (var (key, value) in _properties)
            {
                hash.Add(key);
                hash.Add(value);
            }
            return hash.ToHashCode();
        }
    }
}
