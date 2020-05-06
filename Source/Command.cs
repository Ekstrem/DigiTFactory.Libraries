using System.Windows.Input;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks
{
    /// <summary>
    /// Команда уровня приложения.
    /// </summary>
    /// <typeparam name="T">Тип dto.</typeparam>
    public class Command<T> 
        where T : ICommand
    {
        private readonly CommandToAggregate _metadata;
        private readonly T _dataTransferObject;

        public Command(
            CommandToAggregate metadata,
            T dataTransferObject)
        {
            _metadata = metadata;
            _dataTransferObject = dataTransferObject;
        }

        /// <summary>
        /// Метаданные операции.
        /// </summary>
        public CommandToAggregate Metadata => _metadata;

        /// <summary>
        /// DTO команды.
        /// </summary>
        public T DataTransferObject => _dataTransferObject;
    }
}