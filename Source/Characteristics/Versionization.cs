using System;

namespace Hive.SeedWorks.Characteristics
{
    public sealed class Versionization : IHasVersion
    {
        private readonly int _number;
        private readonly DateTime _stamp;
        private readonly Guid _commandId;

        internal Versionization(int number, DateTime stamp, Guid commandId)
        {
            _number = number;
            _stamp = stamp;
            _commandId = commandId;
        }

        /// <summary>
        /// Номер версии.
        /// </summary>
        public int VersionNumber => _number;

        /// <summary>
        /// Дата создания версии.
        /// </summary>
        public DateTime Stamp => _stamp;

        /// <summary>
        /// Идентификатор комманды, создавшей новую версию.
        /// </summary>
        public Guid CorrelationToken => _commandId;
    }
}