using Hive.SeedWorks.LifeCircle;

namespace Emploee
{
    public interface IHuman : IAggregate<IEmployee>
    {
        string Name { get; }
        string BirthDay { get; }
    }
    
    public 
}