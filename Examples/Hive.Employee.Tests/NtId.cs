namespace Hive.Employee.Tests
{
    internal class NtId : INtId
    {
        private readonly string _domain;
        private readonly string _login;

        public NtId(string login, string domain)
        {
            _login = login;
            _domain = domain;
        }

        public string Domain => _domain;

        public string Login => _login;
    }
}
