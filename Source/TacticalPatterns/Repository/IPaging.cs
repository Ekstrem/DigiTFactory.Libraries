namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Интерфейс постраничной навигации.
    /// </summary>
    public interface IPaging
    {
        /// <summary>
        /// Номер страницы.
        /// </summary>
        int Page { get; }

        /// <summary>
        /// Количество элементов на странице.
        /// </summary>
        int PageSize { get; }
    }
}
