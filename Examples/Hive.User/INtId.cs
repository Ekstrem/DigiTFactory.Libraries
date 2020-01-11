using Hive.SeedWorks.TacticalPatterns;

namespace Hive.User
{
    public interface INtId : IValueObject
    {
        string Domain { get; }
        string Login { get; }
    }
}
