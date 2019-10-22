namespace Hive.SeedWorks.Pipelines.Abstractions
{
    /// <summary>
    /// Операция запроса.
    /// </summary>
    public interface IQuery { }


    /// <summary>
    /// Операция запроса.
    /// </summary>
    /// <typeparam name="T">Тип для запроса.</typeparam>
    public interface IQuery<T>
    : IQuery
    { }
}
