using System;
using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.AbstractAggregate.DynamicTypes
{
    /// <summary>
    /// Динамическая анемичная модель, реализующая IAnemicModel&lt;TBoundedContext&gt;.
    /// Value Object-ы хранятся как словарь, а не как типизированные свойства.
    /// </summary>
    public sealed class DynamicAnemicModel<TBoundedContext> : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly IDictionary<string, IValueObject> _valueObjects;

        /// <summary>
        /// Создание анемичной модели.
        /// </summary>
        public DynamicAnemicModel(
            Guid id,
            long version,
            Guid correlationToken,
            string commandName,
            string subjectName,
            IDictionary<string, IValueObject> valueObjects)
        {
            Id = id;
            Version = version;
            CorrelationToken = correlationToken;
            CommandName = commandName ?? string.Empty;
            SubjectName = subjectName ?? string.Empty;
            _valueObjects = valueObjects ?? new Dictionary<string, IValueObject>();
        }

        /// <inheritdoc />
        public Guid Id { get; }

        /// <inheritdoc />
        public long Version { get; }

        /// <inheritdoc />
        public Guid CorrelationToken { get; }

        /// <inheritdoc />
        public string CommandName { get; }

        /// <inheritdoc />
        public string SubjectName { get; }

        /// <inheritdoc />
        public IDictionary<string, IValueObject> Invariants => _valueObjects;

        /// <inheritdoc />
        public IDictionary<string, IValueObject> GetValueObjects() => _valueObjects;

        /// <summary>
        /// Получить Value Object по имени.
        /// </summary>
        public IValueObject? GetValueObject(string name)
        {
            return _valueObjects.TryGetValue(name, out var vo) ? vo : null;
        }

        /// <summary>
        /// Получить Value Object с приведением к DynamicValueObject.
        /// </summary>
        public DynamicValueObject? GetDynamic(string name)
        {
            return _valueObjects.TryGetValue(name, out var vo) ? vo as DynamicValueObject : null;
        }

        /// <summary>
        /// Создание полной модели.
        /// </summary>
        public static DynamicAnemicModel<TBoundedContext> Create(
            Guid id,
            long version,
            Guid correlationToken,
            string commandName,
            string subjectName,
            IDictionary<string, IValueObject> valueObjects)
        {
            return new DynamicAnemicModel<TBoundedContext>(
                id, version, correlationToken, commandName, subjectName, valueObjects);
        }

        /// <summary>
        /// Создание пустой модели по умолчанию (для нового агрегата).
        /// </summary>
        public static DynamicAnemicModel<TBoundedContext> CreateDefault(Guid id)
        {
            return new DynamicAnemicModel<TBoundedContext>(
                id, 0, Guid.NewGuid(), string.Empty, string.Empty,
                new Dictionary<string, IValueObject>());
        }
    }
}
