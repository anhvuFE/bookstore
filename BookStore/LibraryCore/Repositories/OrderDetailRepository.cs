using LibraryCore.Models;

namespace LibraryCore.Repositories
{
    public class OrderDetailRepository:GenericRepository<OrderDetail>,IOrderDetailRepository
    {
        public OrderDetailRepository(LibraryDBContext context):base(context)
        {
            
        }

        public IList<OrderDetail> GetOrderDetailsByOrderID(int orderid)
        {
            return GetAll().Where(x=>x.OrderId == orderid).ToList();
        }
    }
}
