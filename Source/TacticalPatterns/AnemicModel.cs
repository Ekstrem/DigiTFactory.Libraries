using System;
using System.Collections.Generic;
using System.Linq;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Базовый класс анемичной модели.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public class AnemicModel<TBoundedContext>
        : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
		private Guid _id;
		protected long _version;
		private Guid _correlationToken;

		protected AnemicModel()
		{
		}

		public AnemicModel(Guid id, long version, CommandToAggregate command)
		{
			_id = id;
			_version = version;
			_correlationToken = command.CorrelationToken;
		}

		/// <summary>
		/// Идентификатор сущности.
		/// </summary>
		public Guid Id => _id;

		/// <summary>
		/// Номер версии.
		/// </summary>
		public long Version => _version;

		/// <summary>
		/// Маркер корреляции.
		/// </summary>
		public Guid CorrelationToken => _correlationToken;

		private readonly IDictionary<string, IValueObject> _valueObjects;

        private static IDictionary<string, IValueObject> GetValueObjects(IAnemicModel<TBoundedContext> model)
        {
            var valueObjects = model.ValueObjects;
            var minedValueObjects = model.GetFields();
            foreach (var key in minedValueObjects.Keys.Except(valueObjects.Keys))
            {
                valueObjects.TryAdd(key, minedValueObjects[key]);
            }

            return valueObjects;
        }

        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        protected string ContextName => typeof(TBoundedContext).Name;

        /// <summary>
        /// Словарь объект значений.
        /// </summary>
        public IDictionary<string, IValueObject> ValueObjects => _valueObjects;
    }
}