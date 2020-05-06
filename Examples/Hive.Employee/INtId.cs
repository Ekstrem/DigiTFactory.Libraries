using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.Employee
{
    public interface INtId : IValueObject
    {
        string Domain { get; }
        string Login { get; }
    }
}
