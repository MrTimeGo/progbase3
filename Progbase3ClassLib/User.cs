using System;
using System.Xml.Serialization;

namespace Progbase3ClassLib
{
    [XmlRoot("user")]
    public class User
    {
        public long id;
        public string username;
        public string password;
        public DateTime birthDate;
        public bool isModerator;
        public int gender;  // ISO 5218
        public DateTime createdAt;

        public string GetStringRepresentation()
        {
            const string delimeter = "[~^";
            return $"{id}{delimeter}" +
                $"{username}{delimeter}" +
                $"{password}{delimeter}" +
                $"{birthDate.ToString("o")}{delimeter}" +
                $"{isModerator}{delimeter}" +
                $"{gender}{delimeter}" +
                $"{createdAt.ToString("o")}";
        }
        public static User Parse(string representation)
        {
            const string delimeter = "[~^";
            string[] fields = representation.Split(delimeter);
            User user = new User()
            {
                id = long.Parse(fields[0]),
                username = fields[1],
                password = fields[2],
                birthDate = DateTime.Parse(fields[3]),
                isModerator = bool.Parse(fields[4]),
                gender = int.Parse(fields[5]),
                createdAt = DateTime.Parse(fields[6])
            };
            return user;
        }
    }
}
