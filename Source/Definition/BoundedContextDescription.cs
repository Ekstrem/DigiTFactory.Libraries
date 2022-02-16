namespace Hive.SeedWorks.Definition
{
    /// <summary>
    /// <see cref="IBoundedContextDescription"/>
    /// </summary>
    /// <typeparam name="TBoundedContext"><see cref="IBoundedContext"/></typeparam>
    public class BoundedContextDescription<TBoundedContext> : IBoundedContextDescription
        where TBoundedContext : IBoundedContext
    {
        private readonly string _contextName;
        private readonly int _microserviceVersion;

        public BoundedContextDescription()
        {
            var boundedContextInfo = BoundedContext<TBoundedContext>.GetInfo();
            _contextName = boundedContextInfo.ContextName;
            _microserviceVersion = boundedContextInfo.MicroserviceVersion;
        }
        
        public BoundedContextDescription(string contextName, int microserviceVersion)
        {
            _contextName = contextName;
            _microserviceVersion = microserviceVersion;
        }

        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        public string ContextName => _contextName;

        /// <summary>
        /// Версия микросервиса.
        /// </summary>
        public int MicroserviceVersion => _microserviceVersion;
    }
}