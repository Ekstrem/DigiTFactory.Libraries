using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.Employee
{
    public interface IDepartment : IValueObject
    {
        string Name { get; }
    }
}