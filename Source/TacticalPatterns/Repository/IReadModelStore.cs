using System;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Хранилище записи Read-моделей (проекций).
    /// Используется проекциями для обновления денормализованных данных
    /// на основе доменных событий.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
    public interface IReadModelStore<TBoundedContext, in TReadModel>
        where TBoundedContext : IBoundedContext
        where TReadModel : IReadModel<TBoundedContext>
    {
        /// <summary>
        /// Вставить или обновить Read-модель.
        /// </summary>
        /// <param name="model">Read-модель для сохранения.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        Task UpsertAsync(TReadModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удалить Read-модель по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
