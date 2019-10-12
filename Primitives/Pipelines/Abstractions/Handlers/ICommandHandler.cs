namespace Hive.SeedWorks.Pipelines.Abstractions.Handlers
{
    /// <summary>
    /// Обработчик команд.
    /// </summary>
    /// <typeparam name="TIn">Входящий тип команды.</typeparam>
    /// <typeparam name="TOut">Исходящий тип команды.</typeparam>
    public interface ICommandHandler<in TIn, out TOut>
        : IHandler<TIn, TOut>
        where TIn : ICommand<TOut>
    { }

    /// <summary>
    /// Обработчик команд.
    /// </summary>
    /// <typeparam name="TIn">Входящий тип команды.</typeparam>
    /// <typeparam name="TOut">Исходящий тип команды.</typeparam>
    public interface ICommandHandlerAsync<in TIn, TOut>
        : IHandlerAsync<TIn, TOut>
        where TIn : ICommand<TOut>
    { }
}
