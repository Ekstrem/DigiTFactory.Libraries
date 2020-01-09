using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.TacticalPatterns;
using System;
using Xunit;

namespace Hive.User.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var root = CommandToAggregate.Commit("Test", "Test")
                .PipeTo(cta => ComplexKey.Create(Guid.NewGuid(), 0, cta))
                .PipeTo(ck => new EmployeeRoot(ck, "Пётр", "Иванов", DateTime.MinValue));
            var anemicModel = new AnemicModel<IEmployee>(root);
            var scope = new EmployeeScope(new[] { new Create()})
            var aggregate = Aggregate<IEmployee>.CreateInstance(root, anemicModel, null);
            var create = aggregate.Operations.TryGetValue(nameof(Create), out var operation) ? operation : null;
            var agr = create.Handle(anemicModel, CommandToAggregate.Commit("Test", "Test"));
        }
    }
}
