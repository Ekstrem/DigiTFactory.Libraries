using System;
using System.Collections.Generic;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.Employee.Tests
{
    internal class Employee : 
        AnemicModel<IEmployee>,
        IEmployeeModel
    {
        private readonly IEmployeeRoot _root;
        private readonly IDepartment _department;
        private readonly IUser _user;
        private readonly INtId _ntId;

        public Employee(
            IComplexKey complexKey, 
            IEmployeeRoot root,
            IDepartment department,
            IUser user,
            INtId ntId)
            : base(complexKey, MakeValueObjects(root, department, user, ntId))
        {
            _root = root;
            _department = department;
            _user = user;
            _ntId = ntId;
        }

        public IEmployeeRoot Root => _root;

        public IDepartment Department => _department;

        public IUser User => _user;

        public INtId NtId => _ntId;

        private static IDictionary<string, IValueObject> MakeValueObjects(
            IEmployeeRoot root, IDepartment department, IUser user, INtId ntId)
            => new Dictionary<string, IValueObject>
            {
                {"Root", root},
                {"Department", department},
                {"User", user},
                {"NtId", ntId}
            };

        internal static Employee CreateTest(IComplexKey complexKey)
            => new Employee(
                complexKey,
                new EmployeeRoot(complexKey, "����", "������", DateTime.Now),
                new Department("��������"),
                new UserInfo("Ivanov", "23513q45asdgf23"),
                new NtId("IvanovII", "DO"));

        internal static Employee CreateChangeNameTest(IComplexKey complexKey)
            => new Employee(
                complexKey,
                new EmployeeRoot(complexKey, "����", "������", DateTime.Now),
                new Department("��������"),
                new UserInfo("Ivanov", "23513q45asdgf23"),
                new NtId("IvanovII", "DO"));
    }
}
