using System;
using System.Threading;
using System.Threading.Tasks;
using DigiTFactory.Libraries.SeedWorks.Definition;

namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Провайдер агрегата с явным завершением доставки события.
    /// Дополняет <see cref="IAggregateProvider{TBoundedContext}"/> без изменения его контракта.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    public interface IAggregateProviderWithCompletion<TBoundedContext> :
        IAggregateProviderWithCompletion<TBoundedContext, IAggregate<TBoundedContext>>
        where TBoundedContext : IBoundedContext
    {
    }

    /// <summary>
    /// Провайдер агрегата с явным завершением доставки события.
    /// Дополняет <see cref="IAggregateProvider{TBoundedContext,TAnemicModel}"/> без изменения его контракта.
    /// </summary>
    /// <typeparam name="TBoundedContext">Ограниченный контекст.</typeparam>
    /// <typeparam name="TAnemicModel">Анемичная модель агрегата.</typeparam>
    public interface IAggregateProviderWithCompletion<TBoundedContext, TAnemicModel> :
        IAggregateProvider<TBoundedContext, TAnemicModel>
        where TAnemicModel : IAnemicModel<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        /// <summary>
        /// Загружает агрегат и возвращает вместе с ним <c>Completion</c> — задачу завершения
        /// доставки события в шину.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="version">Версия агрегата.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Агрегат и задача завершения доставки.</returns>
        /// <remarks>
        /// ЗАЧЕМ ЭТОТ МЕТОД: бизнес-методы агрегата остаются синхронными (не «странно
        /// асинхронными»), а команда дожидается долговечной записи события через TCS-мост:
        /// <code>
        /// var (aggregate, completion) = await provider.GetAggregateWithCompletionAsync(id, version, ct);
        /// aggregate.BusinessMethod(); // sync
        /// await completion;           // событие долговечно записано в шину (publish)
        /// </code>
        /// <c>Completion</c> завершается на durable-publish, а НЕ на применении проекций:
        /// MV и Event Store остаются асинхронными подписчиками шины.
        /// </remarks>
        Task<(TAnemicModel Aggregate, Task Completion)> GetAggregateWithCompletionAsync(
            Guid id, long version, CancellationToken cancellationToken);

        /// <summary>
        /// Загружает агрегат и возвращает вместе с ним <c>Completion</c> — задачу завершения
        /// доставки события в шину. Смысл и порядок вызова — см. перегрузку с version.
        /// </summary>
        /// <param name="id">Идентификатор агрегата.</param>
        /// <param name="cancellationToken">Маркер отмены.</param>
        /// <returns>Агрегат и задача завершения доставки.</returns>
        Task<(TAnemicModel Aggregate, Task Completion)> GetAggregateWithCompletionAsync(
            Guid id, CancellationToken cancellationToken);
    }
}
