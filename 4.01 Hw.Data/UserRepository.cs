using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4._01_Hw.Data
{
    public class UserRepository
    {

        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.Password = hash;
            using var ctx = new QAContext(_connectionString);
            ctx.User.Add(user);
            ctx.SaveChanges();
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (isValidPassword)
            {
                return user; 
            }

            return null;
        }

        public User GetByEmail(string email)
        {
            using var ctx = new QAContext(_connectionString);
            return ctx.User.FirstOrDefault(u => u.Email == email);
        }

        public bool EmailAvailable(string email)
        {
            using var ctx = new QAContext(_connectionString);
            return !ctx.User.Any(u => u.Email == email);
        }
    }
}

