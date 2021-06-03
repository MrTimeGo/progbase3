using System;
using System.Security.Cryptography;
using System.Text;
using RPC;
using Progbase3ClassLib;

namespace Miscellaneous
{
    public class Authentication
    {
        RemoteService service;
        public Authentication(RemoteService service)
        {
            this.service = service;
        }
        public User Login(string username, string rawPassword)
        {
            CheckInDataBase(username);
            string password = GetHash(rawPassword);
            User user = service.usersRepo.GetByUsername(username);
            VerifyPassword(password, user.password);

            return user;
        }
        private void VerifyPassword(string inputPassword, string password)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if(comparer.Compare(inputPassword, password) != 0)
            {
                throw new Exception("Invalid username or password");
            }
        }
        private void CheckInDataBase(string username)
        {
            if (!service.usersRepo.UserExists(username))
            {
                throw new Exception("Invalid username or password");
            }
        }
        public void Register(string username, string rawPassword, int gender, DateTime birthDate)
        {
            ValidateUsername(username);
            ValidatePasswordLength(rawPassword);
            ValidateUserAge(birthDate);

            string password = GetHash(rawPassword);

            User user = new User()
            {
                username = username,
                password = password,
                gender = gender,
                birthDate = birthDate,
                createdAt = DateTime.Now,
                isModerator = false
            };

            WriteToDataBase(user);
        }
        private void WriteToDataBase(User user)
        {
            service.usersRepo.Insert(user);
        }
        public static string GetHash(string password)
        {
            SHA256 sha256Hash = SHA256.Create();
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <data.Length;i++)
            {
                sb.Append(data[i].ToString("x2"));
            }

            return sb.ToString();
        }
        private void ValidateUserAge(DateTime birthDate)
        {
            if ((DateTime.Now - birthDate).TotalDays < 12 * 365)
            {
                throw new Exception("User age should be 12+");
            }
        }
        private void ValidatePasswordLength(string password)
        {
            if (password.Length < 6)
            {
                throw new Exception("Password should have more than 5 characters");
            }
        }
        private void ValidateUsername(string username)
        {
            if (service.usersRepo.UserExists(username))
            {
                throw new Exception("Username is busy.");
            }
        }
    }
}
