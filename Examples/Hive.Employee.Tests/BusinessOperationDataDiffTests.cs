using System;
using System.Collections.Generic;
using DigiTFactory.Libraries.SeedWorks.Characteristics;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Xunit;

namespace DigiTFactory.Libraries.Employee.Tests
{
    // Фиксирует контракт диффа BusinessOperationData (BE-136):
    // Commit должен класть в ChangedValueObjects ВСЕ изменившиеся value objects,
    // а не одно изменение. Воспроизводит паттерн BC-Client: record-VO (value equality).
    public class BusinessOperationDataDiffTests
    {
        private sealed record StringVO(string Value) : IValueObject;
        private sealed record IntVO(int Value) : IValueObject;
        private sealed record BoolVO(bool Value) : IValueObject;

        // Модель с VО, которые пересоздаются на каждый вызов GetValueObjects()
        // (как в BC-Client: AnemicModel — record, GetValueObjects строит новый словарь).
        private sealed class M : IAnemicModel<IEmployee>
        {
            public string A { get; init; } = "";
            public int B { get; init; }
            public bool C { get; init; }

            public Guid Id => Guid.Empty;
            public long Version => 0;
            public Guid CorrelationToken => Guid.Empty;
            public string CommandName => string.Empty;
            public string SubjectName => string.Empty;
            public IDictionary<string, IValueObject> Invariants => GetValueObjects();

            public IDictionary<string, IValueObject> GetValueObjects()
                => new Dictionary<string, IValueObject>
                {
                    ["A"] = new StringVO(A),
                    ["B"] = new IntVO(B),
                    ["C"] = new BoolVO(C),
                };
        }

        [Fact]
        public void Commit_returns_all_changed_value_objects()
        {
            var oldModel = new M { A = "", B = 0, C = false };
            var newModel = new M { A = "x", B = 5, C = true };

            var diff = BusinessOperationData<IEmployee, M>
                .Commit<IEmployee, M>(oldModel, newModel)
                .GetValueObjects();

            Assert.Equal(3, diff.Count);
            Assert.Contains("A", diff.Keys);
            Assert.Contains("B", diff.Keys);
            Assert.Contains("C", diff.Keys);
        }

        [Fact]
        public void Commit_returns_only_changed_subset()
        {
            var oldModel = new M { A = "same", B = 1, C = false };
            var newModel = new M { A = "same", B = 9, C = false };

            var diff = BusinessOperationData<IEmployee, M>
                .Commit<IEmployee, M>(oldModel, newModel)
                .GetValueObjects();

            Assert.Single(diff);
            Assert.Contains("B", diff.Keys);
            Assert.DoesNotContain("A", diff.Keys);
            Assert.DoesNotContain("C", diff.Keys);
        }

        [Fact]
        public void Commit_returns_empty_when_nothing_changed()
        {
            var oldModel = new M { A = "same", B = 1, C = true };
            var newModel = new M { A = "same", B = 1, C = true };

            var diff = BusinessOperationData<IEmployee, M>
                .Commit<IEmployee, M>(oldModel, newModel)
                .GetValueObjects();

            Assert.Empty(diff);
        }
    }
}
