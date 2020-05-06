using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.TacticalPatterns.Abstracts;

namespace Hive.Employee
{
    public sealed class ChangeSecondName : DomainCommandExecutor<ChangeSecondName, IEmployee>
    {
        public ChangeSecondName()
            : base(new ChangeSecondNameValidator())
        { }
    }

    public sealed class ChangeSecondNameValidator : IBusinessOperationValidator<IEmployee>
    {
        public string Name => nameof(ChangeSecondName);

        public bool ValidateModel(IAnemicModel<IEmployee> anemicModel)
            => anemicModel is IEmployeeModel model
               && model.Root.SecondName.Length > 2;
    }
}
