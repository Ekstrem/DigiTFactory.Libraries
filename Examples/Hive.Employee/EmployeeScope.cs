using System.Collections.Generic;
using System.Collections.Immutable;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.Employee
{
    public class EmployeeScope : IBoundedContextScope<IEmployee>
    {
        private readonly IReadOnlyDictionary<string, IAggregateBusinessOperation<IEmployee>> _operations;
        private readonly IReadOnlyList<IBusinessEntityValidator<IEmployee>> _validators;

        public EmployeeScope(
            IReadOnlyList<IAggregateBusinessOperation<IEmployee>> operations, 
            IReadOnlyList<IBusinessEntityValidator<IEmployee>> validators)
        {
            _operations = operations.ToImmutableDictionary(k => k.GetType().Name, v => v);
            _validators = validators;
        }

        public IReadOnlyDictionary<string, IAggregateBusinessOperation<IEmployee>> Operations => _operations;

        public IReadOnlyList<IBusinessEntityValidator<IEmployee>> Validators => _validators;
    }
}
