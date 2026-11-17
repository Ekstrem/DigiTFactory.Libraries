namespace DigiTFactory.Libraries.SeedWorks.TacticalPatterns
{
    /// <summary>
    /// Маркер обработчика команды: мутация через агрегат. CommandHandler обязан
    /// инжектить IAggregateProvider в конструктор (QG-210); исключение —
    /// [CommandHandlerNoAggregate].
    /// </summary>
    public interface ICommandHandler
    {
    }
}
