namespace Hive.SeedWorks.Definition
{
    /// <summary>
    /// Описание ограниченного контекста.
    /// </summary>
    public interface IBoundedContextDescription
    {
        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        string ContextName { get; }
        
        /// <summary>
        /// Версия микросервиса.
        /// </summary>
        int MicroserviceVersion { get; }
    }
}