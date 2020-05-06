using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.Employee
{
    public interface IUser : IValueObject
    {
        string Login { get; }
        string Pass { get; }
    }
}