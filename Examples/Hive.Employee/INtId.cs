using Hive.SeedWorks.TacticalPatterns;

namespace Hive.Employee
{
    public interface INtId : IValueObject
    {
        string Domain { get; }
        string Login { get; }
    }
}
