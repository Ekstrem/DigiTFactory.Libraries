using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Сервис перестроения Read-модели (проекций) из Event Store.
    /// Читает все события в хронологическом порядке и прогоняет через Projection Handler.
    /// Идемпотентен: повторный вызов безопасен (события с Version <= текущей пропускаются).
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IRebuildService<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Перестроить все проекции из Event Store.
        /// </summary>
        /// <param name="ct">Маркер отмены.</param>
        Task RebuildAsync(CancellationToken ct = default);
    }
}
