using Hive.SeedWorks.Characteristics;
using System;

namespace Hive.SeedWorks
{
    /// <summary>
    /// Сведения о команде к агрегату.
    /// </summary>
    public class CommandToAggregate : ICommandMetadata
    {
        private readonly Guid _id;
        private readonly long _version;
        private readonly Guid _correlationToken;
        private readonly Guid _branchId;
        private readonly string _commandName;
        private readonly string _subjectName;

        private CommandToAggregate(
            Guid id,
            long version,
            Guid correlationToken,
            Guid branchId,
            string commandName,
            string subjectName)
        {
            _id = id;
            _version = version;
            _correlationToken = correlationToken;
            _branchId = branchId;
            _commandName = commandName;
            _subjectName = subjectName;
        }
        
        /// <summary>
        /// Идентификатор агрегата.
        /// </summary>
        public Guid Id => _id;

        /// <summary>
        /// Определяет версию. Ожидаемое использование - дата создания версии в милисекундах.
        /// Является приведением <see cref="DateTimeOffset"/> к формату времени
        /// Unix в милисекундах.
        /// </summary>
        public long Version => _version;

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationToken => _correlationToken;

        /// <summary>
        /// Идентификатор ветви.
        /// </summary>
        public Guid BranchId => _branchId;

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
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="version">Версия команды.</param>
        /// <param name="correlationToken">Маркер корреляции.</param>
        /// <param name="branchId">Идентификатор ветки.</param>
        /// <param name="commandName">Имя команды инициатора доменного события.</param>
        /// <param name="subjectName">Имя инициировавшего событие.</param>
        /// <param name="instanceId">Идентификатор экземпляра.</param>
        /// <param name="magorVersion">Минорная версия микросервиса.</param>
        /// <param name="minorVersion">Минорная версия микросервиса.</param>
        /// <returns></returns>
        public static CommandToAggregate Commit(
            Guid id, long version,
            Guid correlationToken, Guid branchId,
            string commandName, string subjectName)
            => new CommandToAggregate(id, version, correlationToken, branchId, commandName, subjectName);
    }
}
