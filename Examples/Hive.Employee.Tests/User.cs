namespace Hive.Employee.Tests
{
    public class UserInfo : IUser
    {
        private readonly string _pass;
        private readonly string _login;

        public UserInfo(string login, string pass)
        {
            _pass = pass;
            _login = login;
        }

        public string Login => _login;

        public string Pass => _pass;
    }
}
