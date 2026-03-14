using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Репозиторий команд (CUD).
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TModel">Анемичная модель.</typeparam>
    public interface ICommandRepository<TBoundedContext, in TModel>
        where TBoundedContext : IBoundedContext
        where TModel : AnemicModel<TBoundedContext>
    {
        /// <summary>
        /// Добавляет запись.
        /// </summary>
        void Add(TModel entity);

        /// <summary>
        /// Обновляет запись.
        /// </summary>
        void Update(TModel entity);

        /// <summary>
        /// Удаляет запись.
        /// </summary>
        void Delete(TModel entity);
    }
}
