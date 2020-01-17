using System;

namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Класс версионирования.
    /// Частичная реализация версии для передачи в другие параметры.
    /// </summary>
    public sealed class Versioning : IHasVersion
    {
        private readonly long _stamp;
        private readonly Guid _correlationToken;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="stamp">Дата время вызова команды.</param>
        /// <param name="correlationToken">Маркер корреляции.</param>
        internal Versioning(DateTimeOffset stamp, Guid correlationToken)
        {
            _stamp = stamp.ToUnixTimeMilliseconds();
            _correlationToken = correlationToken;
        }

        /// <summary>
        /// Дата создания версии. Одновременно определяет версию.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        public long Version => _stamp;

        /// <summary>
        /// Идентификатор комманды, создавшей новую версию.
        /// </summary>
        public Guid CorrelationToken => _correlationToken;
    }
}
