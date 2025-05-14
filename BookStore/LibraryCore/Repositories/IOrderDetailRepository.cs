using LibraryCore.Models;

namespace LibraryCore.Repositories
{
    public interface IOrderDetailRepository:IGenericRepository<OrderDetail>{
    
        public IList<OrderDetail> GetOrderDetailsByOrderID(int userid);
    }
}
