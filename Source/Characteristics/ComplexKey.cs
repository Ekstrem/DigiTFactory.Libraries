using System;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.Characteristics
{
    internal sealed class ComplexKey : IHasComplexKey
    {
        private readonly Guid _id;
        private readonly int _number;
        private readonly DateTime _stamp;
        private readonly Guid _commandId;

        internal ComplexKey(Guid id, int number, CommandToAggregate command)
        {
            _id = id;
            _number = number;
            _stamp = DateTime.Now;
            _commandId = command.CommandId;
        }

        public Guid Id => _id;

        public int VersionNumber => _number;

        public DateTime Stamp => _stamp;

        public Guid CommandId => _commandId;

        public static IHasComplexKey Create(Guid id, int versionNumber, CommandToAggregate command)
            => new ComplexKey(id, versionNumber, command);
    }
}