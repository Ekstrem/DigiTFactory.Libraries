namespace Hive.Employee.Tests
{
    public class Department : IDepartment
    {
        private readonly string _name;

        public Department(string name)
        {
            _name = name;
        }

        public string Name => _name;
    }
}
