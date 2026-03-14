using System;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;
using Microsoft.EntityFrameworkCore;

namespace DigiTFactory.Libraries.ReadRepository.Postgres
{
    /// <summary>
    /// PostgreSQL реализация IReadModelStore через EF Core.
    /// Используется проекциями для записи/обновления Read-моделей.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TReadModel">Тип Read-модели.</typeparam>
    public class PostgresReadModelStore<TBoundedContext, TReadModel>
        : IReadModelStore<TBoundedContext, TReadModel>
        where TBoundedContext : IBoundedContext
        where TReadModel : class, IReadModel<TBoundedContext>
    {
        private readonly DbContext _dbContext;

        public PostgresReadModelStore(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task UpsertAsync(TReadModel model, CancellationToken cancellationToken = default)
        {
            var entry = _dbContext.Entry(model);

            if (entry.State == EntityState.Detached)
            {
                var existing = await _dbContext.Set<TReadModel>()
                    .FindAsync(new object[] { entry.Property("Id").CurrentValue! }, cancellationToken);

                if (existing != null)
                {
                    _dbContext.Entry(existing).CurrentValues.SetValues(model);
                }
                else
                {
                    await _dbContext.Set<TReadModel>().AddAsync(model, cancellationToken);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.Set<TReadModel>()
                .FindAsync(new object[] { id }, cancellationToken);

            if (entity != null)
            {
                _dbContext.Set<TReadModel>().Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
