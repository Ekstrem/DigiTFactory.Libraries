using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.DynamicTypes
{
    /// <summary>
    /// Динамическая коллекция Value Object-ов.
    /// Представляет коллекционный VO (например, Messages) как список DynamicValueObject.
    /// </summary>
    public sealed class DynamicCollectionValueObject : IValueObject, IEnumerable<DynamicValueObject>
    {
        private readonly List<DynamicValueObject> _items;

        /// <summary>
        /// Создание коллекции из типового имени и списка элементов.
        /// </summary>
        public DynamicCollectionValueObject(string typeName, IEnumerable<DynamicValueObject> items)
        {
            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            _items = items?.ToList() ?? new List<DynamicValueObject>();
        }

        /// <summary>
        /// Имя типа коллекции (из метаданных).
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Количество элементов.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Все элементы коллекции.
        /// </summary>
        public IReadOnlyList<DynamicValueObject> Items => _items;

        /// <summary>
        /// Создать новую коллекцию, добавив элементы из другой коллекции.
        /// </summary>
        public DynamicCollectionValueObject Append(DynamicCollectionValueObject other)
        {
            var combined = new List<DynamicValueObject>(_items);
            combined.AddRange(other._items);
            return new DynamicCollectionValueObject(TypeName, combined);
        }

        public IEnumerator<DynamicValueObject> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object? obj)
        {
            if (obj is not DynamicCollectionValueObject other) return false;
            if (TypeName != other.TypeName) return false;
            return _items.SequenceEqual(other._items);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(TypeName);
            foreach (var item in _items)
                hash.Add(item);
            return hash.ToHashCode();
        }
    }
}
