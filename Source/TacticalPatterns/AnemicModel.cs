using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
		protected int _number;
		private DateTime _stamp;
		private Guid _correlationToken;

		protected AnemicModel()
		{
		}

		public AnemicModel(Guid id, int number, CommandToAggregate command)
		{
			_id = id;
			_number = number;
			_stamp = DateTime.Now;
			_correlationToken = command.CorrelationToken;
		}

		/// <summary>
		/// Идентификатор сущности.
		/// </summary>
		public Guid Id => _id;

		/// <summary>
		/// Номер версии.
		/// </summary>
		public int VersionNumber => _number;

		/// <summary>
		/// Временная метка.
		/// </summary>
		public DateTime Stamp => _stamp;

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