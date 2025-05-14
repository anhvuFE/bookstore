using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCore.Repositories
{
    public class CartRepository:GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(LibraryDBContext context):base(context)
        {
            
        }
        public IList<Cart> FindByUserId(int userId)
        {
            return dbSet.Include(x=>x.Book.Category).Where(x=>x.User.UserId == userId).ToList();
        }

        public void DeleteAllByUserId(int userId)
        {
            dbSet.RemoveRange(FindByUserId(userId));
        }

        public Cart FindByBookIdAndUserId(int bookId,int userId)
        {
            return dbSet.FirstOrDefault(x => x.BookId == bookId && x.UserId==userId);
        }

        public Cart FindCartByIdWithNav(int cartId)
        {
            return dbSet.Include(x=>x.Book).FirstOrDefault(x => x.CartId==cartId);
        }
    }
}
