using System;
using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.SeedWorks.DomainModel
{
    /// <summary>
    /// Базовая реализация агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    public class Aggregate<TBoundedContext> : 
        IAggregate<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        protected readonly IAnemicModel<TBoundedContext> AnemicModel;

        protected Aggregate(IAnemicModel<TBoundedContext> anemicModel) => AnemicModel = anemicModel;

        #region IHasComplexKey interface implamintation

        /// <summary>
        /// Имеет идентификатор.
        /// </summary>
        public Guid Id => AnemicModel.Id;

        /// <summary>
        /// Определяет версию. Ожидаемое использование - дата создания версии в милисекундах.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        public long Version => AnemicModel.Version;

        #endregion

        /// <summary>
        /// Метаданные о предыдущей операции,
        /// породившей данную версию.
        /// </summary>
        public ICommandMetadata PreviousOperation => AnemicModel.PreviousOperation;

        public string BoundedContextName => AnemicModel.BoundedContextName;

        /// <summary>
        /// Объекты значения.
        /// </summary>
        public IDictionary<string, IValueObject> ValueObjects => AnemicModel.ValueObjects;
        
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"{Id} {Version}";

        public static IAggregate<TBoundedContext> Create(IAnemicModel<TBoundedContext> anemicModel)
            => new Aggregate<TBoundedContext>(anemicModel);

    }
}
