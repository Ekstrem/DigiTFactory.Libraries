using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace DigiTFactory.Libraries.Employee
{
    public sealed class Create : AggregateBusinessOperation<Create, IEmployee>
    {
        public Create() : base(null) { }        
    }
}
