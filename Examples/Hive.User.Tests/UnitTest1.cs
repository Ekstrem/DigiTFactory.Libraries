using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.TacticalPatterns;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Hive.User.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var anemicModel = Employee.CreateTest(NewComplexKey());
            var operations = new List<IAggregateBusinessOperation<IEmployee>>
            {
                new Create(),
                new ChangeSecondName(),
            };
            var validators = new[] {new NamesValidator(),}.ToList();
            var scope = new EmployeeScope(operations, validators);
            var aggregate = Aggregate<IEmployee>.CreateInstance(anemicModel, anemicModel, scope);

            var create = aggregate.Operations.TryGetValue(nameof(Create), out var operation) ? operation : null;
            Assert.NotNull(create);
            var aggregateCreateResult = create.Handle(anemicModel, NewCommand(), scope);
            Assert.NotNull(aggregateCreateResult);

            var aggregateResult = aggregateCreateResult.Aggregate
                .Operations[nameof(ChangeSecondName)]
                .Handle(Employee.CreateChangeNameTest(NewComplexKey()), NewCommand(), scope);
            Assert.NotNull(aggregateResult);
        }

        private CommandToAggregate NewCommand()
            => CommandToAggregate.Commit(Guid.NewGuid(), "Test", "Test");

        private IComplexKey NewComplexKey()
            => ComplexKey.CreateWithUsingCorrelationToken(NewCommand());
    }
}
