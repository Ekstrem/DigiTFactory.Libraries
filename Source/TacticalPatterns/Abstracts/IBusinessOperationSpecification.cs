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

        /// <summary>
        /// Причина, по которой объект не проходит проверку данной спецификацией,
        /// которая отражается в шине доменных сообщений.
        /// </summary>
        string Reason { get; }
    }
}