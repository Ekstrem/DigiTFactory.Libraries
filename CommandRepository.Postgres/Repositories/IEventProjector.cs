using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.CommandRepository.Postgres.Entities;

namespace DigiTFactory.Libraries.CommandRepository.Postgres.Repositories
{
    /// <summary>
    /// Доменный проектор одного сохранённого события. Используется Rebuild-сервисом
    /// для пере-вывода Read-модели из Event Store.
    /// </summary>
    public interface IEventProjector
    {
        /// <summary>Применяет одно событие к проекции (идемпотентно).</summary>
        Task ProjectAsync(DomainEventEntry entry, CancellationToken ct = default);
    }
}
