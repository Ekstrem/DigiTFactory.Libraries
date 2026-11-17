using System.Linq;
using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Расширение IReadRepository для хранилищ, поддерживающих IQueryable (напр. Postgres/EF Core).
    /// Даёт произвольные запросы поверх базового GetByIdAsync/GetAllAsync/CountAsync.
    /// Хранилища без IQueryable (Redis/Scylla) реализуют только IReadRepository.
    /// </summary>
    public interface IQueryableReadRepository<TBoundedContext, TReadModel>
        : IReadRepository<TBoundedContext, TReadModel>
        where TBoundedContext : IBoundedContext
        where TReadModel : IReadModel<TBoundedContext>
    {
        IQueryable<TReadModel> GetQueryable();
    }
}
