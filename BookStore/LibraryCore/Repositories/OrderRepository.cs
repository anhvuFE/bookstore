using LibraryCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCore.Repositories
{
    public class OrderRepository:GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(LibraryDBContext context) : base(context)
        {
            
        }

        public IList<Order> GetOrderByUserId(int userId)
        {
            return dbSet.Include(x=>x.OrderDetails).ThenInclude(od=>od.Book).ThenInclude(od=>od.Category).Where(x=>x.UserId==userId).ToList();
        }

        public int[] CountOrdersByMonth()
        {
            var result = context.Orders
                .GroupBy(l => l.OrderDate.Month)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToArray();

            return result;
        }

        public IList<Order> GetAllOrdersNav()
        {
            return dbSet.Include(x=>x.User).Include(x => x.OrderDetails).ThenInclude(od => od.Book).ThenInclude(od=>od.Category).ToList();
        }
    }
}
