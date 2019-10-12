namespace Hive.SeedWorks.Events
{
    /// <summary>
    /// Шина событий.
    /// </summary>
    public interface IEventBus :
        IEventBusProducer,
        IEventBusConsumer
    {
    }
}