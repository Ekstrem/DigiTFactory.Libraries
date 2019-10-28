using System;
using Hive.SeedWorks.Events;

namespace Hive.SeedWorks.Business
{
    public static class Extention
    {
        public static IHasVersion GenNextVersion(int version, CommandToAggregate command)
            => new Versionization(version, DateTime.Now, command.CommandId);

        public static IHasComplexKey GetNextComplexKey(this CommandToAggregate command, Guid id, int version)
            => new ComplexKey(id, version, command);

        public static IHasComplexKey CreateNewVersion(this CommandToAggregate command)
            => new ComplexKey(command.CommandId, 0, command);
    }
}