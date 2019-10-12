using System;
using Hive.SeedWorks.LifeCircle;

namespace Hive.SeedWorks.BoundedContexts
{
    /// <summary>
    /// Интерфейс для указания принадлежности агрегатов
    /// предметной области Контрагентов.
    /// </summary>
    public interface IPartnerSharedKernel : IBoundedContext { }

    [Obsolete("На случай кастомизации контекстов.")]
    public interface ISapMdmPartnerBoundedContext : IPartnerSharedKernel { }

    [Obsolete("На случай кастомизации контекстов.")]
    public interface IPaydoxPartnerBoundedContext : IPartnerSharedKernel { }
}