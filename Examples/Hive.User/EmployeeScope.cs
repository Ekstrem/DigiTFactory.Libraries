using System.Collections.Generic;
using System.Collections.Immutable;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.User
{
    public class EmployeeScope : IBoundedContextScope<IEmployee>
    {
        private readonly IReadOnlyDictionary<string, IAggregateBusinessOperationFactory<IEmployee>> _operations;
        private readonly IReadOnlyList<IBusinessValidator<IEmployee>> _validators;

        public EmployeeScope(
            IReadOnlyList<IAggregateBusinessOperation<IEmployee>> operations, 
            IReadOnlyList<IBusinessEntityValidator<IEmployee>> validators)
        {
            _operations = operations.ToImmutableDictionary(k => k.GetType().Name, v => v);
            _validators = validators;
        }

        public IReadOnlyDictionary<string, IAggregateBusinessOperationFactory<IEmployee>> Operations => _operations;

        public IReadOnlyList<IBusinessValidator<IEmployee>> Validators => _validators;
    }
}
