using System;

namespace Hive.SeedWorks.Business
{
    /// <summary>
    /// Комплексный ключ агрегата.
    /// </summary>
    public interface IHasComplexKey : IHasKey<Guid>, IHasVersion { }
}