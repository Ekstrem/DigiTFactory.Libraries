namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IBusinessOperationSpecification<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Проверка соответствующей спецификации объекта.
        /// </summary>
        /// <param name="obj">Объект проверки.</param>
        /// <returns>Результат проверки.</returns>
        bool IsSatisfiedBy(IAnemicModel<TBoundedContext> obj);
    }
}