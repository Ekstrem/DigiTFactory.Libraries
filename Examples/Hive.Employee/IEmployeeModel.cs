using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.Employee
{
    /// <summary>
    /// Модель сотрудника компании.
    /// </summary>
    public interface IEmployeeModel : IAnemicModel<IEmployee>
    {
        /// <summary>
        /// <see cref="IEmployeeRoot"/>
        /// </summary>
        IEmployeeRoot Root { get; }

        /// <summary>
        /// <see cref="IDepartment"/>
        /// </summary>
        IDepartment Department { get; }

        /// <summary>
        /// <see cref="IUser"/>
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// <see cref="INtId"/>
        /// </summary>
        INtId NtId { get; }
    }
}
