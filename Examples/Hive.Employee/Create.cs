using Hive.SeedWorks.TacticalPatterns;

namespace Hive.Employee
{
    public sealed class Create : DomainCommandExecutor<Create, IEmployee>
    {
        public Create() : base(null) { }        
    }
}
