using System.Linq.Expressions;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.Specification
{
    /// <summary>
    /// Спецификация сортировки.
    /// </summary>
    /// <typeparam name="TEntity">Агрегат.</typeparam>
    public interface IOrderSpecification<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Функция подготовки условия сортировки.
        /// </summary>
        /// <returns>Дерево выражений для OrderBy.</returns>
        Expression OrderBy();
    }
}