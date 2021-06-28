using Hive.SeedWorks.Characteristics;

namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IHasCommandMetadata
    {
        /// <summary>
        /// Метаданные о предыдущей операции,
        /// породившей данную версию.
        /// </summary>
        ICommandMetadata PreviousOperation { get; }
    }
}
