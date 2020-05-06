using System.Collections.Generic;
using System.Collections.Immutable;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.Employee
{
    public class EmployeeScope : IBoundedContextScope<IEmployee>
    {
        private readonly IReadOnlyDictionary<string, IDomainCommandExecutor<IEmployee>> _operations;
        private readonly IReadOnlyList<IBusinessEntityValidator<IEmployee>> _validators;

        public EmployeeScope(
            IReadOnlyList<IDomainCommandExecutor<IEmployee>> operations, 
            IReadOnlyList<IBusinessEntityValidator<IEmployee>> validators)
        {
            _operations = operations.ToImmutableDictionary(k => k.GetType().Name, v => v);
            _validators = validators;
        }

        public IReadOnlyDictionary<string, IDomainCommandExecutor<IEmployee>> Operations => _operations;

        public IReadOnlyList<IBusinessEntityValidator<IEmployee>> Validators => _validators;
    }
}
