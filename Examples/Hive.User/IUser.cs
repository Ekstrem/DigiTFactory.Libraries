using Hive.SeedWorks.TacticalPatterns;

namespace Hive.User
{
    public interface IUser : IValueObject
    {
        string Login { get; }
        string Pass { get; }
    }
}