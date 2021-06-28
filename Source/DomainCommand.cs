using System;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks
{
    /// <summary>
    /// Команда уровня доменной модели.
    /// </summary>
    public class DomainCommand : ICommand
    {
        private readonly CommandToAggregate _metadata;
        private readonly IHasComplexKey _key;
        private readonly IValueObject[] _valueObjects;

        public DomainCommand(
            CommandToAggregate metadata,
            IHasComplexKey key,
            params IValueObject[] valueObjects)
        {
            _metadata = metadata;
            _key = key;
            _valueObjects = valueObjects;
        }

        public DomainCommand(
            CommandToAggregate metadata,
            Guid id,
            long version,
            params IValueObject[] valueObjects)
        {
            _metadata = metadata;
            _key = ComplexKey.Create(id, version);
            _valueObjects = valueObjects;
        }

        /// <summary>
        /// Метаданные операции.
        /// </summary>
        public CommandToAggregate Metadata => _metadata;
        
        /// <summary>
        /// Состовной ключ бизнес сущности.
        /// </summary>
        public IHasComplexKey Key => _key;
        
        /// <summary>
        /// Объекты значения.
        /// </summary>
        public IValueObject[] ValueObjects => _valueObjects;
    }
}