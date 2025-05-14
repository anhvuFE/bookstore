using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.Models;

namespace LibraryCore.Repositories
{
    internal class UserRepository:GenericRepository<User>,IUserRepository

    {
        public UserRepository(LibraryDBContext context) : base(context)
        {
            
        }

        public string AddUser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();

            string generatedId = user.UserId.ToString();
            return generatedId;
        }

        public User FindUserByName(string userName)
        {
            return context.Users.FirstOrDefault(x => x.Username == userName);
        }
    }
}
