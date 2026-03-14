using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.EntityFrameworkCore;

namespace DigiTFactory.Libraries.ReadRepository.Postgres
{
    /// <summary>
    /// PostgreSQL реализация IReadRepository через EF Core.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
    public class PostgresReadRepository<TBoundedContext, TReadModel>
        : IReadRepository<TBoundedContext, TReadModel>
        where TBoundedContext : IBoundedContext
        where TReadModel : class, IReadModel<TBoundedContext>
    {
        private readonly DbContext _dbContext;

        public PostgresReadRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<TReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TReadModel>().FindAsync(new object[] { id }, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TReadModel>> GetAllAsync(
            IPaging paging,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TReadModel>()
                .Skip((paging.Page - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TReadModel>().LongCountAsync(cancellationToken);
        }
    }
}
