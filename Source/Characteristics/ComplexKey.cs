using System;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.Characteristics
{
    internal sealed class ComplexKey : IHasComplexKey, IComplexKey
    {
        private readonly Guid _id;
        private readonly int _number;
        private readonly DateTime _stamp;
        private readonly Guid _correlationToken;

        internal ComplexKey(Guid id, int number, CommandToAggregate command)
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
        
        public static IComplexKey Create(Guid id, int versionNumber)
            => new ComplexKey(id, versionNumber, null);

        public static IHasComplexKey Create(Guid id, int versionNumber, CommandToAggregate command)
            => new ComplexKey(id, versionNumber, command);
    }

    internal interface IComplexKey : IHasKey, IHasVersion { }
}
