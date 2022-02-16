namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Субъект изменений.
    /// </summary>
    public interface ICommandSubject
    {
        /// <summary>
        /// Имя метода агрегата, который вызывает команда.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// Имя субъекта бизнес-операции.
        /// </summary>
        string SubjectName { get; }
    }
} 