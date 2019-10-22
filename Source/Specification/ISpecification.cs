using System;
using System.Linq.Expressions;

namespace Hive.SeedWorks.Specification
{
    /// <summary>
    /// Спецификация для объектов.
    /// </summary>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Возаращает выражение для рассчёта выволняется ли условие
        /// </summary>
        Expression<Func<T, bool>> IsSatisfiedBy();
    }
}