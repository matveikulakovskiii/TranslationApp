using System.Collections.Generic;

namespace TranslationApp.Services
{
    public static class UserRepository
    {
        private static Dictionary<string, string> users = new Dictionary<string, string>();

        public static void CreateUser(string email, string password)
        {
            if (!users.ContainsKey(email))
            {
                users[email] = password;
            }
        }

        public static bool UserExists(string email, string password)
        {
            return users.ContainsKey(email) && users[email] == password;
        }
    }
}
