using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.Employee
{
    public interface IUser : IValueObject
    {
        string Login { get; }
        string Pass { get; }
    }
}