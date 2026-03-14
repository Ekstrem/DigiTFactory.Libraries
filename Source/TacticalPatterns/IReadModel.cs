using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Маркерный интерфейс Read-модели (проекции).
    /// Read-модель — денормализованное представление данных, оптимизированное для чтения.
    /// Заполняется через проекции доменных событий.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IReadModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
    }
}
