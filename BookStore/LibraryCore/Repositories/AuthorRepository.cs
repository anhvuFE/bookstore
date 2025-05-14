using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.Models;

namespace LibraryCore.Repositories
{
    public class AuthorRepository:GenericRepository<Author>,IAuthorRepository{
        public AuthorRepository(LibraryDBContext context):base(context)
        {
            
        }
    }
}
