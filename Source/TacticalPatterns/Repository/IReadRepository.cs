using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Репозиторий чтения проекций (Read Model).
    /// Предоставляет доступ к денормализованным Read-моделям.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
    public interface IReadRepository<TBoundedContext, TReadModel>
        where TBoundedContext : IBoundedContext
        where TReadModel : IReadModel<TBoundedContext>
    {
        /// <summary>
        /// Получить Read-модель по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Read-модель или null, если не найдена.</returns>
        Task<TReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить страницу Read-моделей.
        /// </summary>
        /// <param name="paging">Параметры постраничной навигации.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Список Read-моделей.</returns>
        Task<IReadOnlyList<TReadModel>> GetAllAsync(IPaging paging, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить количество Read-моделей.
        /// </summary>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Количество записей.</returns>
        Task<long> CountAsync(CancellationToken cancellationToken = default);
    }
}
