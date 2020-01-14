using System.Collections.Generic;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.Employee
{
    public class NamesValidator : IBusinessEntityValidator<IEmployee>
    {
        public bool ValidateModel(IAnemicModel<IEmployee> anemicModel)
            => ValidateRoot(anemicModel.Invariants)
               && ValidateUser(anemicModel.Invariants)
               && ValidateDepartment(anemicModel.Invariants)
               && ValidateNtId(anemicModel.Invariants);

        bool ValidateRoot(IDictionary<string, IValueObject> invariants)
            => invariants.TryGetValue("Root", out IValueObject rootVo)
               && rootVo is IEmployeeRoot root
               && root.FirstName.Length > 2
               && root.SecondName.Length > 2;

        bool ValidateNtId(IDictionary<string, IValueObject> invariants)
            => invariants.TryGetValue("NtId", out IValueObject vo)
               && vo is INtId ntId
               && ntId.Domain.Length >= 2
               && ntId.Login.Length > 4;

        bool ValidateDepartment(IDictionary<string, IValueObject> invariants)
            => invariants.TryGetValue("Department", out IValueObject vo)
               && vo is IDepartment department
               && department.Name.Length > 2;

        bool ValidateUser(IDictionary<string, IValueObject> invariants)
            => invariants.TryGetValue("User", out IValueObject vo)
               && vo is IUser user
               && user.Login.Length > 2;
    }
}
