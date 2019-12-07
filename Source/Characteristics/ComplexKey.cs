using System;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.Characteristics
{
    public sealed class ComplexKey : IHasComplexKey, IComplexKey
    {
        private readonly Guid _id;
        private readonly long _version;
        private readonly Guid _correlationToken;

        private ComplexKey(Guid id, long version, Guid correlationToken)
        {
            _id = id;
            _version = version;
            _correlationToken = correlationToken;
        }

        private ComplexKey(Guid id, CommandToAggregate command)
            : this(id, command.Version, command.CorrelationToken) { }

        /// <summary>
        /// Идентификатор сущности.
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
        /// Создание экземляра комплексного ключа.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="version">Версия агрегата.</param>
        /// <param name="correlationToken">Токен корреляции.</param>
        /// <returns></returns>
        public static IComplexKey Create(Guid id, long version, Guid correlationToken)
            => new ComplexKey(id, version, correlationToken);

        /// <summary>
        /// Создание экземляра комплексного ключа.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="command">Комманда к агрегату.</param>
        /// <returns></returns>
        public static IComplexKey Create(Guid id, CommandToAggregate command)
            => new ComplexKey(id, command);

        /// <summary>
        /// Создание экземпляра комплексного ключа полностью из данных о команде к агрегату,
        /// т.е. с использованием маркера корреляции.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static IComplexKey CreateWithUsingCorrelationToken(CommandToAggregate command)
            => new ComplexKey(command.CorrelationToken, command.Version, command.CorrelationToken);
    }
}
