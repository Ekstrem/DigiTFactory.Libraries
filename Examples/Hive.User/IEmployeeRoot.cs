using System;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.User
{
    public interface IEmployeeRoot : IAggregateRoot<IEmployee>
    {
        string FirstName { get; }
        string SecondName { get; }
        DateTime BirthDay { get; }
    }

    public class EmployeeRoot : IEmployeeRoot
    {
        private readonly IHasComplexKey _key;
        private readonly string _firstName;
        private readonly string _secondName;
        private readonly DateTime _birthDay;

        public EmployeeRoot(
            IHasComplexKey key,
            string firstName, string secondName, DateTime birthDay)
        {
            _key = key;
            _firstName = firstName;
            _secondName = secondName;
            _birthDay = birthDay;
        }

        public Guid Id => _key.Id;
        public int VersionNumber => _key.VersionNumber;
        public DateTime Stamp => _key.Stamp;
        public Guid CorrelationToken => _key.CorrelationToken;

        public string FirstName => _firstName;

        public string SecondName => _secondName;

        public DateTime BirthDay => _birthDay;
    }
}