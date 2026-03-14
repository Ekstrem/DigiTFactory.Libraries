using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using DigiTFactory.Libraries.ReadRepository.Scylla.Configuration;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.Extensions.Logging;

namespace DigiTFactory.Libraries.ReadRepository.Scylla
{
    /// <summary>
    /// ScyllaDB/Cassandra реализация IReadRepository.
    /// Хранит Read-модели как JSON в таблице с partition key = id.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
    public class ScyllaReadRepository<TBoundedContext, TReadModel>
        : IReadRepository<TBoundedContext, TReadModel>
        where TBoundedContext : IBoundedContext
        where TReadModel : class, IReadModel<TBoundedContext>
    {
        private readonly ISession _session;
        private readonly ScyllaReadStoreOptions _options;
        private readonly ILogger<ScyllaReadRepository<TBoundedContext, TReadModel>> _logger;
        private readonly string _modelType;

        public ScyllaReadRepository(
            ISession session,
            ScyllaReadStoreOptions options,
            ILogger<ScyllaReadRepository<TBoundedContext, TReadModel>> logger)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _modelType = typeof(TReadModel).Name;
        }

        /// <inheritdoc />
        public async Task<TReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cql = $"SELECT data FROM {_options.Keyspace}.{_options.TableName} " +
                       "WHERE id = ? AND model_type = ?";

            var statement = new SimpleStatement(cql, id, _modelType);
            var result = await _session.ExecuteAsync(statement);
            var row = result.FirstOrDefault();

            if (row == null)
                return default;

            var json = row.GetValue<string>("data");
            return JsonSerializer.Deserialize<TReadModel>(json);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TReadModel>> GetAllAsync(
            IPaging paging,
            CancellationToken cancellationToken = default)
        {
            var cql = $"SELECT data FROM {_options.Keyspace}.{_options.TableName} " +
                       "WHERE model_type = ? ALLOW FILTERING";

            var statement = new SimpleStatement(cql, _modelType)
                .SetPageSize(paging.PageSize);

            var result = await _session.ExecuteAsync(statement);

            return result
                .Select(row => JsonSerializer.Deserialize<TReadModel>(row.GetValue<string>("data")))
                .ToList();
        }

        /// <inheritdoc />
        public async Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            var cql = $"SELECT COUNT(*) FROM {_options.Keyspace}.{_options.TableName} " +
                       "WHERE model_type = ? ALLOW FILTERING";

            var statement = new SimpleStatement(cql, _modelType);
            var result = await _session.ExecuteAsync(statement);
            var row = result.FirstOrDefault();

            return row?.GetValue<long>("count") ?? 0;
        }
    }
}
