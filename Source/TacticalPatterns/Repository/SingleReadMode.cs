namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns.Repository
{
    /// <summary>
    /// Режим чтения единичной записи.
    /// </summary>
    public enum SingleReadMode
    {
        /// <summary>
        /// Единственная запись (выбросит исключение если записей больше одной).
        /// </summary>
        Single = 0,

        /// <summary>
        /// Первая запись.
        /// </summary>
        First = 1,

        /// <summary>
        /// Последняя запись.
        /// </summary>
        Last = 2
    }
}
