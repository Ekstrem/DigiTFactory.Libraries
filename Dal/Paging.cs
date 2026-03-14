using DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository;

namespace DigiTFactory.Libraries.Dal
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
