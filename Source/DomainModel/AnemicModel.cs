using System;
using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.DomainModel
{
    /// <summary>
    /// Базовый класс анемичной модели агрегата с корнем агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    public abstract class AnemicModel<TBoundedContext, TRoot> :
        AnemicModel<TBoundedContext>,
        IAnemicModel<TBoundedContext, TRoot>
        where TBoundedContext : IBoundedContext
        where TRoot : class, IAggregateRoot<TBoundedContext>
    {
        protected AnemicModel(
            ICommandMetadata commandMetadata,
            IDictionary<string, IValueObject> valueObjects)
            : base(commandMetadata, valueObjects)
        { }
        
        protected AnemicModel(
            ICommandMetadata commandMetadata,
            params IValueObject[] valueObjects)
            : base(commandMetadata, valueObjects)
        { }

        /// <summary>
        /// Корень агрегата как фиксированное объект-значение,
        /// с заданным именем.
        /// </summary>
        public TRoot Root => (TRoot) (ValueObjects.TryGetValue(typeof(TRoot).Name, out var root) ? root : null);
    }

    /// <summary>
    /// Базовый класс анемичной модели агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    public abstract class AnemicModel<TBoundedContext> :
        IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly ICommandMetadata _commandMetadata;
        private readonly IDictionary<string, IValueObject> _valueObjects;
        private string _boundedContextName;

        protected AnemicModel(
            ICommandMetadata commandMetadata,
            IDictionary<string, IValueObject> valueObjects)
        {
            _commandMetadata = commandMetadata;
            _valueObjects = valueObjects;
        }
        
        protected AnemicModel(
            ICommandMetadata commandMetadata,
            params IValueObject[] valueObjects)
            : this(commandMetadata, valueObjects.ToDictionary(k => k.GetType().Name, v => v))
        { }

        #region IHasComplexKey interface implamintation

        /// <summary>
        /// Имеет идентификатор.
        /// </summary>
        public Guid Id => _commandMetadata.Id;

        /// <summary>
        /// Определяет версию. Ожидаемое использование - дата создания версии в милисекундах.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        public long Version => _commandMetadata.Version;

        #endregion

        /// <summary>
        /// Метаданные о предыдущей операции,
        /// породившей данную версию.
        /// </summary>
        public ICommandMetadata PreviousOperation => _commandMetadata;

        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        public string BoundedContextName => typeof(TBoundedContext).Name;

        /// <summary>
        /// Объекты значения.
        /// </summary>
        public IDictionary<string, IValueObject> ValueObjects => _valueObjects;
        
        
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"{_commandMetadata.Id} {_commandMetadata.Version} {_commandMetadata.CorrelationToken}";
    }
}