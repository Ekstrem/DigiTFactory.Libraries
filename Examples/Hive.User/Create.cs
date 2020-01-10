using System;
using Hive.SeedWorks.Characteristics;
using Hive.SeedWorks.Events;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.User
{
    public sealed class Create : AggregateBusinessOperation<Create, IEmployee>
    {
        public Create() : base(null) { }        
    }
}
