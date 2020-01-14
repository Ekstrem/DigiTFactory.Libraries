using Hive.SeedWorks.TacticalPatterns;

namespace Hive.Employee
{
    public interface IDepartment : IValueObject
    {
        string Name { get; }
    }
}