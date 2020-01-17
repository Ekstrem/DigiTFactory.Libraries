using Hive.SeedWorks.TacticalPatterns.Repository;

namespace Hive.Dal
{
    /// <summary>
    /// Постраничная навигация
    /// </summary>
    public class Paging : IPaging
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Кол-во элементов на странице
        /// </summary>
        public int PageSize { get; set; }
    }
}
