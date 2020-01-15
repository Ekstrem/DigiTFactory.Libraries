using System;
using System.Collections.Generic;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Базовый класс анемичной модели.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public abstract class AnemicModel<TBoundedContext> :
        IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
		private Guid _id;
		protected long _version;
		private Guid _correlationToken;
		private IDictionary<string, IValueObject> _invariants;

		protected AnemicModel()
		{
		}

		public AnemicModel(
			Guid id,
			long version,
			Guid correlationToken)
		{
			_id = id;
			_version = version;
			_correlationToken = correlationToken;
		}

        /// <summary>
        /// Имеет идентификатор.
        /// </summary>
        public Guid Id => _id;

        /// <summary>
        /// Определяет версию. Ожидаемое использование - дата создания версии в милисекундах.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        public long Version => _version;

        /// <summary>
        /// Идентификатор комманды, создавшей новую версию.
        /// </summary>
        public Guid CorrelationToken => _correlationToken;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> Invariants => GetInvariants();

        private IDictionary<string, IValueObject> GetInvariants()
		{
			return _invariants ?? (_invariants = this.GetFields());
		}
	}
}
