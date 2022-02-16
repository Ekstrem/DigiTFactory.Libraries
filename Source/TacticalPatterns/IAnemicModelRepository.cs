using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hive.SeedWorks.Definition;

namespace Hive.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Репозиторий получения из хранилища анемичной модели агрегата.
    /// В норме - зависимость для провайдера агрегатов <see cref="IAggregateProvider{TBoundedContext}"/>
    /// </summary>
    /// <typeparam name="TBoundedContext">Тип ограниченного контекста.</typeparam>
    /// <typeparam name="TAnemicModel">Тип анемичной модели.</typeparam>
    public interface IAnemicModelRepository<TBoundedContext, TAnemicModel>
        where TAnemicModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Получить стрим анемичных моделей по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор анемичной модели.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Список анемичных моделей.</returns>
        Task<List<TAnemicModel>> GetById(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Получить стрим анемичных моделей по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор анемичной модели.</param>
        /// <param name="version">Версия агрегата.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Список анемичных моделей.</returns>
        Task<TAnemicModel> GetByIdAndVersion(Guid id, long version, CancellationToken cancellationToken);

        /// <summary>
        /// Получить стрим анемичных моделей по идентификатору.
        /// </summary>
        /// <param name="correlationToken">Маркер корреляции.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Список анемичных моделей.</returns>
        Task<TAnemicModel> GetByCorrelationToken(Guid correlationToken, CancellationToken cancellationToken);
        
        
    }
}