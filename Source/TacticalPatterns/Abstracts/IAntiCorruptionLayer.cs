namespace Hive.SeedWorks.TacticalPatterns.Abstracts
{
    public interface IAntiCorruptionLayer<T> where T : ICommand
    {
        DomainCommand Translate(Command<T> input);
    }
}