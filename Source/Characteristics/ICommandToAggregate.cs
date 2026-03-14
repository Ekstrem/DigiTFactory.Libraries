using System;

namespace DigiTFactory.Libraries.SeedWorks.Characteristics
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