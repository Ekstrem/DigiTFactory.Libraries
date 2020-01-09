using Hive.SeedWorks.TacticalPatterns;

namespace Hive.User
{
    public interface IDepartment : IValueObject
    {
        string Name { get; }
    }
}