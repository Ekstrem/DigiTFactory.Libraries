using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DigiTFactory.Libraries.Testing
{
    /// <summary>
    /// Coverage-assertion (QG-702): каждый бизнес-метод агрегата (возвращающий AggregateResult)
    /// обязан иметь хотя бы один тест. Проверяется рефлексией в тест-проекте — это покрытие,
    /// а не синтаксис, поэтому живёт тестом, а не Roslyn-анализатором.
    /// </summary>
    public static class AggregateCoverage
    {
        /// <summary>
        /// Проверяет, что у каждого публичного метода агрегата, возвращающего AggregateResult,
        /// есть тест-метод, имя которого содержит имя бизнес-метода.
        /// </summary>
        /// <typeparam name="TAggregate">Тип агрегата (реализация IAggregate/IRecordAggregate).</typeparam>
        /// <param name="testAssembly">Сборка тестов (Assembly.GetExecutingAssembly()).</param>
        /// <param name="isTestMethod">Предикат «это тест» (напр. имеет атрибут Fact/Theory).</param>
        public static void Verify<TAggregate>(Assembly testAssembly, Func<MethodInfo, bool> isTestMethod)
        {
            if (testAssembly == null) throw new ArgumentNullException(nameof(testAssembly));
            if (isTestMethod == null) throw new ArgumentNullException(nameof(isTestMethod));

            var aggregateMethods = typeof(TAggregate)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.ReturnType.Name.StartsWith("AggregateResult"))
                .Select(m => m.Name)
                .OrderBy(n => n)
                .ToList();

            var testNames = testAssembly
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(isTestMethod)
                .Select(m => m.Name)
                .ToList();

            var missing = aggregateMethods
                .Where(m => !testNames.Any(t => t.Contains(m)))
                .ToList();

            if (missing.Count > 0)
            {
                throw new InvalidOperationException(
                    "Нет тестов на бизнес-методы агрегата (QG-702): " + string.Join(", ", missing));
            }
        }

        /// <summary>
        /// Проверяет, что у каждого инварианта/валидатора домена (IBusinessOperationValidator/
        /// IBusinessOperationAssertion/IBusinessOperationSpecification) есть тест — то есть
        /// покрыт не только happy-path, но и кейс отказа инварианта.
        /// </summary>
        /// <param name="domainAssembly">Сборка домена (typeof(Aggregate).Assembly).</param>
        /// <param name="testAssembly">Сборка тестов (Assembly.GetExecutingAssembly()).</param>
        /// <param name="isTestMethod">Предикат «это тест» (напр. имеет атрибут Fact/Theory).</param>
        public static void VerifyInvariants(Assembly domainAssembly, Assembly testAssembly, Func<MethodInfo, bool> isTestMethod)
        {
            if (domainAssembly == null) throw new ArgumentNullException(nameof(domainAssembly));
            if (testAssembly == null) throw new ArgumentNullException(nameof(testAssembly));
            if (isTestMethod == null) throw new ArgumentNullException(nameof(isTestMethod));

            var validatorNames = SafeGetTypes(domainAssembly)
                .Where(t => t.IsClass && !t.IsAbstract &&
                            t.GetInterfaces().Any(i => i.Name.StartsWith("IBusinessOperation")))
                .Select(t => t.Name)
                .OrderBy(n => n)
                .ToList();

            var testNames = testAssembly
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(isTestMethod)
                .Select(m => m.Name)
                .ToList();

            var missing = validatorNames
                .Where(n => !testNames.Any(t => t.Contains(n)))
                .ToList();

            if (missing.Count > 0)
            {
                throw new InvalidOperationException(
                    "Нет тестов на инварианты/валидаторы (QG-702): " + string.Join(", ", missing));
            }
        }

        private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null).Select(t => t!);
            }
        }
    }
}

// 0.1.0 — coverage-assertion (QG-702, публикуется через testing.yml)
