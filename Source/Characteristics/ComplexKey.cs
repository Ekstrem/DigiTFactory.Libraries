using System;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.Characteristics
{
    public sealed class ComplexKey : IHasComplexKey, IComplexKey
    {
        private Guid _id;
        private int _number;
        private DateTime _stamp;
        private Guid _correlationToken;

		internal ComplexKey()
		{
		}
		internal ComplexKey(Guid id, int number, CommandToAggregate command)
        {
            _id = id;
            _number = number;
            _stamp = DateTime.Now;
            _correlationToken = command?.CorrelationToken ?? Guid.NewGuid();
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

        public static ComplexKey Create(Guid id, int versionNumber)
            => new ComplexKey(id, versionNumber, null);

        public static IHasComplexKey Create(Guid id, int versionNumber, CommandToAggregate command)
            => new ComplexKey(id, versionNumber, command);
    }

    public interface IComplexKey : IHasKey, IHasVersion { }
}
