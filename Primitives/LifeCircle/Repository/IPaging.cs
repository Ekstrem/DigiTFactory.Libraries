namespace Hive.SeedWorks.LifeCircle.Repository
{
    public interface IPaging
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// Кол-во элементов на странице
        /// </summary>
        int PageSize { get; set; }
    }
}