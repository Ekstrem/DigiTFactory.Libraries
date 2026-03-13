using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.Employee
{
    public interface IDepartment : IValueObject
    {
        string Name { get; }
    }
}