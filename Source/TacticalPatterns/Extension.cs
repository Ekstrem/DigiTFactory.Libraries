using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hive.SeedWorks.Monads;

namespace Hive.SeedWorks.LifeCircle
{
    internal static class Extension
    {
        // Должно ускорить работу с рефлексией.
        private static readonly IDictionary<Type, IEnumerable<FieldInfo>> FieldsInfo
            = new ConcurrentDictionary<Type, IEnumerable<FieldInfo>>();
        
        internal static IDictionary<string, IValueObject> SetFields(
            this IDictionary<string, IValueObject> fields,
            object obj,
            params string[] exclude)
        {
            obj
                .PipeTo(p => p.GetType())
                .PipeTo(p => FieldsInfo[p])
                .Do(a =>
                {
                    var fieldsInfo = a.ToDictionary(k => k.Name, v => v);
                    foreach (var key in fields.Keys.Except(exclude))
                    {

                        fieldsInfo[key].SetValue(obj, fields[key]);
                    }
                });

            return fields;
        }


        internal static IDictionary<string, IValueObject> GetFields(this object obj)
            => obj
                .PipeTo(p => p.GetType())
                .Do(a =>
                {
                    if (!FieldsInfo.ContainsKey(a))
                    {
                        FieldsInfo.Add(
                            a,
                            a.UnderlyingSystemType.GetRuntimeFields()
                                .Select(m => m)
                                .Where(f => f.FieldType.GetInterface(typeof(IValueObject).Name) != null));
                    }
                })
                .PipeTo(p => FieldsInfo[p])
                .ToDictionary(
                    field => field.Name,
                    field => (IValueObject) field.GetValue(obj));
    }
}