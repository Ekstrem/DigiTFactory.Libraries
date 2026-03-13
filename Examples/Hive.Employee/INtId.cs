using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.Employee
{
    public interface INtId : IValueObject
    {
        string Domain { get; }
        string Login { get; }
    }
}
