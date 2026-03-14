using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.ReadRepository.Redis.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DigiTFactory.Libraries.ReadRepository.Redis
{
    /// <summary>
    /// Redis реализация IReadRepository.
    /// Хранит Read-модели как JSON по ключу {KeyPrefix}{TypeName}:{Id}.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
    public class RedisReadRepository<TBoundedContext, TReadModel>
        : IReadRepository<TBoundedContext, TReadModel>
        where TBoundedContext : IBoundedContext
        where TReadModel : class, IReadModel<TBoundedContext>
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly RedisReadStoreOptions _options;
        private readonly ILogger<RedisReadRepository<TBoundedContext, TReadModel>> _logger;
        private readonly string _keyPattern;

        public RedisReadRepository(
            IConnectionMultiplexer redis,
            RedisReadStoreOptions options,
            ILogger<RedisReadRepository<TBoundedContext, TReadModel>> logger)
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _keyPattern = $"{_options.KeyPrefix}{typeof(TReadModel).Name}:";
        }

        /// <inheritdoc />
        public async Task<TReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(GetKey(id));

            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<TReadModel>(value!);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TReadModel>> GetAllAsync(
            IPaging paging,
            CancellationToken cancellationToken = default)
        {
            var server = GetServer();
            var keys = server.Keys(pattern: $"{_keyPattern}*")
                .Skip((paging.Page - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToArray();

            if (keys.Length == 0)
                return Array.Empty<TReadModel>();

            var db = _redis.GetDatabase();
            var values = await db.StringGetAsync(keys);

            return values
                .Where(v => v.HasValue)
                .Select(v => JsonSerializer.Deserialize<TReadModel>(v!))
                .ToList();
        }

        /// <inheritdoc />
        public Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            var server = GetServer();
            var count = server.Keys(pattern: $"{_keyPattern}*").LongCount();
            return Task.FromResult(count);
        }

        private string GetKey(Guid id) => $"{_keyPattern}{id}";

        private IServer GetServer()
        {
            var endpoints = _redis.GetEndPoints();
            return _redis.GetServer(endpoints[0]);
        }
    }
}
