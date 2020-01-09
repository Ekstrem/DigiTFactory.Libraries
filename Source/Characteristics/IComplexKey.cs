using System;

namespace Hive.SeedWorks.Characteristics
{
    //public interface IComplexKey : IHasKey<Guid>, IHasVersion { }

    /// <summary>
    /// Комплексный ключ агрегата с рекомендуемыми типами данных.
    /// </summary>
    public interface IComplexKey :
        IComplexKey<Guid, long>,
        IHasKey, 
        IHasVersion
    {
    }

    /// <summary>
    /// Комплексный ключ агрегата.
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора агрегата.</typeparam>
    /// <typeparam name="TVersion">Тип идентификатора версии.</typeparam>
    public interface IComplexKey<out TKey, out TVersion> :
        IHasKey<TKey>,
        IHasVersion<TVersion>
    {
    }
}
