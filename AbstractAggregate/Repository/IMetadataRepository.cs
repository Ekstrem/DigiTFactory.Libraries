using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;

namespace DigiTFactory.Libraries.AbstractAggregate.Repository
{
    /// <summary>
    /// Репозиторий метаданных агрегатов.
    /// Абстракция над хранилищем (БД, файл, API).
    /// </summary>
    public interface IMetadataRepository
    {
        /// <summary>
        /// Получить метаданные агрегата по имени.
        /// </summary>
        Task<AggregateMetadata?> GetByNameAsync(string aggregateName, CancellationToken ct = default);

        /// <summary>
        /// Получить метаданные агрегата по идентификатору.
        /// </summary>
        Task<AggregateMetadata?> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Получить все метаданные агрегатов.
        /// </summary>
        Task<IReadOnlyList<AggregateMetadata>> GetAllAsync(CancellationToken ct = default);

        /// <summary>
        /// Сохранить (создать или обновить) метаданные агрегата.
        /// </summary>
        Task SaveAsync(AggregateMetadata metadata, CancellationToken ct = default);
    }
}
