using System;
using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Сведения о команде к агрегату.
    /// </summary>
    public class CommandToAggregate : IHasVersion, ICommandSubject
    {
        private readonly Guid _correlationToken;
        private readonly string _commandName;
        private readonly string _subjectName;
        private readonly long _version;

        private CommandToAggregate(
            Guid correlationToken,
            string commandName,
            string subjectName)
        {
            _correlationToken = correlationToken;
            _commandName = commandName;
            _subjectName = subjectName;
            _version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationToken => _correlationToken;

        /// <summary>
        /// Определяет версию. Ожидаемое использование - дата создания версии в милисекундах.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        public long Version => _version;

        /// <summary>
        /// Имя метода агрегата, который вызывает команда.
        /// </summary>
        public string CommandName => _commandName;

        /// <summary>
        /// Имя субъекта бизнес-операции.
        /// Мапится на NameLastModification.
        /// </summary>
        public string SubjectName => _subjectName;

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"{_commandName} {_subjectName}";

        /// <summary>
        /// Создаст структуру <see cref="CommandToAggregate"/>.
        /// </summary>
        /// <param name="correlationToken">Маркер корреляции.</param>
        /// <param name="commandName">Имя команды инициатора доменного события.</param>
        /// <param name="nameLastModification">Имя инициировавшего событие.</param>
        /// <returns></returns>
        public static CommandToAggregate Commit(
            Guid correlationToken, string commandName, string nameLastModification)
            => new CommandToAggregate(correlationToken, commandName, nameLastModification);
    }
}
