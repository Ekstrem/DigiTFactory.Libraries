using Hive.SeedWorks.TacticalPatterns;

namespace Hive.Employee
{
    public interface IUser : IValueObject
    {
        string Login { get; }
        string Pass { get; }
    }
}