using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hive.SeedWorks.Monads;
using Hive.SeedWorks.TacticalPatterns;

namespace Hive.SeedWorks.LifeCircle
{
    public static class ValueObjectHelper
    {
        public static IDictionary<string, IValueObject> GetValueObjects(this object obj)
            => obj
                .PipeTo(a => new List<PropertyInfo>(a
                    .GetType()
                    .GetProperties()
                    .Where(f =>
                        !f.PropertyType.IsValueType &&
                        f.GetValue(obj) != null &&
                        (IsVO(f.PropertyType)))))
                .Select(s => s.GetValue(obj))
                .ToDictionary(
                    k => k.GetType().Name,
                    v => (IValueObject)v);

        private static bool IsVO(Type type) => type.GetInterface(nameof(IValueObject)) != null;
    }
}