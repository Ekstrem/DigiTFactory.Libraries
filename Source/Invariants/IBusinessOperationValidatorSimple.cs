using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Валидатор бизнес-операции с одним параметром типа ограниченного контекста.
    /// Используется для проверки возможности выполнения бизнес-операции.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IBusinessOperationValidator<TBoundedContext> : IValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Имя бизнес-операции.
        /// </summary>
        string Name { get; }
    }
}
