using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    public class DashboardModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IList<int> OrdersByMonth { get; set; }
        public int CountUser { get; set; }
        public int CountOrder { get; set; }
        public int CountBook { get; set; }
        public void OnGet()
        {
            OrdersByMonth = _unitOfWork.OrderRepository.CountOrdersByMonth();
            CountUser = _unitOfWork.UserRepository.GetAll().Count;
            CountOrder = _unitOfWork.OrderRepository.GetAll().Where(x => x.OrderDate.Date == DateTime.Now.Date).Count();
            CountBook = _unitOfWork.BookRepository.GetAll().Count;
        }
    }
}
