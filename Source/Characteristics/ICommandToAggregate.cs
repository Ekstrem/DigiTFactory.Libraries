using System;

namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Описание метаданных команды.
    /// </summary>
    public interface ICommandToAggregate :
        IHasVersion,
        IHasCorrelationToken,
        ICommandSubject
    {
        
    }
}