using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.Models;

namespace LibraryCore.Repositories
{
    public interface ICartRepository:IGenericRepository<Cart>
    {
        public IList<Cart> FindByUserId(int userId);
        public void DeleteAllByUserId(int userId);

        public Cart FindByBookIdAndUserId(int bookId,int userId);
        public Cart FindCartByIdWithNav(int cartId);
    }
}
