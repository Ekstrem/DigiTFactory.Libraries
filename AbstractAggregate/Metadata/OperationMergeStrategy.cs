namespace DigiTFactory.Libraries.AbstractAggregate.Metadata
{
    /// <summary>
    /// Стратегия слияния Value Object-ов при выполнении бизнес-операции.
    /// </summary>
    public enum OperationMergeStrategy
    {
        /// <summary>
        /// Заменить только указанные в AffectedValueObjects.
        /// </summary>
        ReplaceAffected = 0,

        /// <summary>
        /// Для коллекций — добавить элементы, для скалярных VO — заменить.
        /// </summary>
        AppendCollections = 1,

        /// <summary>
        /// Полная замена всей модели.
        /// </summary>
        FullReplace = 2
    }
}
