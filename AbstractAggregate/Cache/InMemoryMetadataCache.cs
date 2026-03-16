using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.AbstractAggregate.Metadata;
using DigiTFactory.Libraries.AbstractAggregate.Repository;

namespace DigiTFactory.Libraries.AbstractAggregate.Cache
{
    /// <summary>
    /// In-memory кэш метаданных агрегатов.
    /// Загружает при первом обращении и хранит в памяти до инвалидации.
    /// </summary>
    public sealed class InMemoryMetadataCache : IMetadataCache
    {
        private readonly IMetadataRepository _repository;
        private readonly ConcurrentDictionary<string, AggregateMetadata> _cache = new(StringComparer.OrdinalIgnoreCase);

        public InMemoryMetadataCache(IMetadataRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc />
        public async Task<AggregateMetadata> GetOrLoadAsync(string aggregateName, CancellationToken ct = default)
        {
            if (_cache.TryGetValue(aggregateName, out var cached))
                return cached;

            var metadata = await _repository.GetByNameAsync(aggregateName, ct)
                ?? throw new InvalidOperationException(
                    $"Aggregate metadata '{aggregateName}' not found in repository.");

            _cache.TryAdd(aggregateName, metadata);
            return metadata;
        }

        /// <inheritdoc />
        public void Invalidate(string aggregateName)
        {
            _cache.TryRemove(aggregateName, out _);
        }

        /// <inheritdoc />
        public void InvalidateAll()
        {
            _cache.Clear();
        }
    }
}
