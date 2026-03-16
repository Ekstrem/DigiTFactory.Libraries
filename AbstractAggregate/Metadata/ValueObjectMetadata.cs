using System.Collections.Generic;

namespace DigiTFactory.Libraries.AbstractAggregate.Metadata
{
    /// <summary>
    /// Метаданные Value Object (секция 2 Aggregate Design Canvas).
    /// </summary>
    public sealed class ValueObjectMetadata
    {
        /// <summary>
        /// Имя Value Object (например "Root", "Actor", "Message").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Является ли данный VO корнем агрегата (IAggregateRoot).
        /// </summary>
        public bool IsAggregateRoot { get; set; }

        /// <summary>
        /// Является ли VO коллекцией (IEnumerable).
        /// </summary>
        public bool IsCollection { get; set; }

        /// <summary>
        /// Свойства Value Object.
        /// </summary>
        public IReadOnlyList<PropertyMetadata> Properties { get; set; } = [];
    }
}
