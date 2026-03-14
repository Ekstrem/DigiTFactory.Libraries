using System;

namespace DigiTFactory.Libraries.SeedWorks.Result
{
    /// <summary>
    /// Результат выполнения доменной операции.
    /// </summary>
    public interface IDomainOperationResult
    {
        /// <summary>
        /// Результат операции.
        /// </summary>
        DomainOperationResultEnum Result { get; }
        
        /// <summary>
        /// Причина ошибки.
        /// </summary>
        string Reason { get; }
    }
}