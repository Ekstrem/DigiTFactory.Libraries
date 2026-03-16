using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;

namespace DigiTFactory.Libraries.AbstractAggregate.Cache
{
    /// <summary>
    /// Кэш метаданных агрегатов.
    /// Загружает метаданные из репозитория и кэширует для повторного использования.
    /// </summary>
    public interface IMetadataCache
    {
        /// <summary>
        /// Получить метаданные из кэша или загрузить из репозитория.
        /// </summary>
        Task<AggregateMetadata> GetOrLoadAsync(string aggregateName, CancellationToken ct = default);

        /// <summary>
        /// Инвалидировать кэш для конкретного агрегата.
        /// </summary>
        void Invalidate(string aggregateName);

        /// <summary>
        /// Инвалидировать весь кэш.
        /// </summary>
        void InvalidateAll();
    }
}
