using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.Models;

namespace LibraryCore.Repositories
{
    public interface IUserRepository:IGenericRepository<User>
    {
        string AddUser(User user);

        User FindUserByName(string userName);
    }
}
