namespace Hive.SeedWorks.LifeCircle
{
    /// <summary>
    /// Анемичная модель ограниченного контекста для Фабрики создания агрегата.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контест.</typeparam>
    public interface IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    { }
}