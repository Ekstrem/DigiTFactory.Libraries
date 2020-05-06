namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    /// <summary>
    /// Границы ограниченного контекста.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IBoundedContextScope<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Имя субдомена.
        /// </summary>
        string SubdomainName { get; }
        
        /// <summary>
        /// Пояснения к ограниченному контексту.
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// Версия микосервиса.
        /// </summary>
        int MicroserviceVersion { get; }
    }
}
