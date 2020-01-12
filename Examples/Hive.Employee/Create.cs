using Hive.SeedWorks.TacticalPatterns;

namespace Hive.Employee
{
    public sealed class Create : AggregateBusinessOperation<Create, IEmployee>
    {
        public Create() : base(null) { }        
    }
}
