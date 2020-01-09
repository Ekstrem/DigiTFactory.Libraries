using Hive.SeedWorks.TacticalPatterns;

namespace Hive.User
{
    public interface IEmployeeModel : IAnemicModel<IEmployee>
    {
        IDepartment Department { get; }
        IUser User { get; }
        INtId NtId { get; }
    }
}