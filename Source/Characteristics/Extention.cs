using System;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.Characteristics
{
    public static class Extention
    {
        public static IHasVersion GenNextVersion(int version, CommandToAggregate command)
            => new Versionization(version, DateTime.Now, command.CorrelationToken);

        public static IHasComplexKey GetNextComplexKey(this CommandToAggregate command, Guid id, int version)
            => new ComplexKey(id, version, command);

        public static IHasComplexKey CreateNewVersion(this CommandToAggregate command)
            => new ComplexKey(command.CorrelationToken, 0, command);
    }
}