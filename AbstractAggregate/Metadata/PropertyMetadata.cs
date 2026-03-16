namespace DigiTFactory.Libraries.AbstractAggregate.Metadata
{
    /// <summary>
    /// Метаданные свойства Value Object.
    /// </summary>
    public sealed class PropertyMetadata
    {
        /// <summary>
        /// Имя свойства.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Имя CLR-типа: "Guid", "string", "int", "long", "bool", "DateTime", "decimal",
        /// "double", "float", "byte" или FQN enum-типа.
        /// </summary>
        public string TypeName { get; set; } = "string";

        /// <summary>
        /// Обязательное ли свойство.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Значение по умолчанию (JSON-encoded).
        /// </summary>
        public string? DefaultValue { get; set; }
    }
}
