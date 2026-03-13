using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.Employee
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
