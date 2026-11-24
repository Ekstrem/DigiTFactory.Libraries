using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Read-модель с версией события. Позволяет идемпотентный upsert:
    /// применяется только более новая версия.
    /// </summary>
    public interface IVersionedReadModel<TBoundedContext> : IReadModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>Версия события, создавшего текущее состояние проекции.</summary>
        long Version { get; }
    }
}
