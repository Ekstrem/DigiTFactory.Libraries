using System;

namespace Hive.SeedWorks.Characteristics
{
    public interface IComplexKey : IComplexKey<Guid> { }

    public interface IComplexKey<out TKey> :
        IHasKey<TKey>,
        IVersion
    {
    }
}
