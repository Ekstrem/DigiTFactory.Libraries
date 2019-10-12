using System;

namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Сведения о команде к агрегату.
    /// </summary>
    public class CommandToAggregate
    {
        private readonly Guid _commandId;
        private readonly string _commandName;
        private readonly string _subjectName;

        public CommandToAggregate(
            Guid commandId,
            string commandName,
            string subjectName)
        {
            _commandId = commandId;
            _commandName = commandName;
            _subjectName = subjectName;
        }

        public CommandToAggregate(
            string commandName,
            string subjectName)
        {
            _commandId = Guid.NewGuid();
            _commandName = commandName;
            _subjectName = subjectName;
        }

        /// <summary>
        /// Уникальный идентификатор комманды.
        /// </summary>
        public Guid CommandId => _commandId;

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
        /// <param name="commandName">Имя команды инициатора доменного события.</param>
        /// <param name="nameLastModification">Имя инициировавшего событие.</param>
        /// <returns></returns>
        public static CommandToAggregate Commit(string commandName, string nameLastModification)
            => new CommandToAggregate(commandName, nameLastModification);

        /// <summary>
        /// Создаст структуру <see cref="CommandToAggregate"/>.
        /// </summary>
        /// <param name="id">Идентификатор команды.</param>
        /// <param name="commandName">Имя команды инициатора доменного события.</param>
        /// <param name="nameLastModification">Имя инициировавшего событие.</param>
        /// <returns></returns>
        public static CommandToAggregate Commit(
            Guid id, string commandName, string nameLastModification)
            => new CommandToAggregate(commandName, nameLastModification);
    }
}