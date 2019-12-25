using System;

namespace Hive.SeedWorks.Characteristics
{
    /// <summary>
    /// Комплексный ключ агрегата.
    /// </summary>
    public interface IComplexKey : IHasKey<Guid>, IHasVersion { }
}
