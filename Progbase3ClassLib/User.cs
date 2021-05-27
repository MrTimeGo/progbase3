using System;

namespace Progbase3ClassLib
{
    public class User
    {
        public long id;
        public string username;
        public string password;
        public DateTime birthDate;
        public bool isModerator;
        public int gender;  // ISO 5218
        public DateTime createdAt;
    }
}
