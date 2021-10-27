namespace Hive.SeedWorks.Definition
{
    /// <summary>
    /// Описание ограниченного контекста <see cref="T"/>
    /// </summary>
    /// <typeparam name="T">Тип ограниченного контекста.</typeparam>
    public class BoundedContext<T> : IBoundedContextDescription
        where T : IBoundedContext
    {
        private static IBoundedContextDescription s_bcInfo;
        
        /// <summary>
        /// Имя ограниченного контекста.
        /// </summary>
        public string ContextName => typeof(T).Name;

        /// <summary>
        /// Версия микросервиса.
        /// </summary>
        public int MicroserviceVersion => typeof(T).Assembly.GetName().Version.Major;

        /// <summary>
        /// Получение информации об ограниченном контексте.
        /// </summary>
        /// <returns>Тип ограниченного контекста.</returns>
        public static IBoundedContextDescription GetInfo() => s_bcInfo ??= new BoundedContext<T>();
    }
}