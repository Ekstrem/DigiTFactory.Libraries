using Hive.SeedWorks.TacticalPatterns.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hive.SeedWorks
{
    /// <summary>
    /// Служебный класс валидирования бизнес-операций.
    /// </summary>
    public class BusinessOperationValidator<TBoundedContext>
        where TBoundedContext : IBoundedContext
    {
        private readonly Dictionary<IBusinessOperationSpecification<TBoundedContext>, bool> _validators;

        private BusinessOperationValidator(
            IAnemicModel<TBoundedContext> model,
            params IBusinessOperationSpecification<TBoundedContext>[] specifications)
        {
            _validators = specifications
                .ToDictionary(
                    k => k,
                    v => v.IsSatisfiedBy(model));
        }

        public bool Result => _validators.Values.All(p => p);

        public IEnumerable<string> GetFailedValidatorsNames()
            => _validators
                .Where(f => !f.Value)
                .Select(m => m.Key.Reason);

        public static BusinessOperationValidator<TBoundedContext> Create(
            IAnemicModel<TBoundedContext> model,
            params IBusinessOperationSpecification<TBoundedContext>[] specifications)
            => new BusinessOperationValidator<TBoundedContext>(model, specifications);

        public static BusinessOperationValidator<TBoundedContext> Create<TSpec>(
            IAnemicModel<TBoundedContext> model)
            where TSpec : IBusinessOperationSpecification<TBoundedContext>
            => new BusinessOperationValidator<TBoundedContext>(model, Activator.CreateInstance<TSpec>());

        public static BusinessOperationValidator<TBoundedContext> Create<TSpec1, TSpec2>(
            IAnemicModel<TBoundedContext> model)
            where TSpec1 : IBusinessOperationSpecification<TBoundedContext>
            where TSpec2 : IBusinessOperationSpecification<TBoundedContext>
            => new BusinessOperationValidator<TBoundedContext>(
                model,
                Activator.CreateInstance<TSpec1>(),
                Activator.CreateInstance<TSpec2>());

        public static BusinessOperationValidator<TBoundedContext> Create<TSpec1, TSpec2, TSpec3>(
            IAnemicModel<TBoundedContext> model)
            where TSpec1 : IBusinessOperationSpecification<TBoundedContext>
            where TSpec2 : IBusinessOperationSpecification<TBoundedContext>
            where TSpec3 : IBusinessOperationSpecification<TBoundedContext>
            => new BusinessOperationValidator<TBoundedContext>(
                model,
                Activator.CreateInstance<TSpec1>(),
                Activator.CreateInstance<TSpec2>(),
                Activator.CreateInstance<TSpec3>());
    }
}
