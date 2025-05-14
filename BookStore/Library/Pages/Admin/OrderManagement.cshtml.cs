using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    public class OrderManagementModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderManagementModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IList<Order> Orders { get; set; }

        public void OnGet()
        {
            Orders = _unitOfWork.OrderRepository.GetAllOrdersNav();
        }

        public IActionResult OnGetOrderDetails(int orderId)
        {
            var order = _unitOfWork.OrderRepository.GetAllOrdersNav()
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }
            return Partial("_OrderDetailsPartial", order.OrderDetails);
        }

        public IActionResult OnPostEditOrder()
        {
            int orderId = int.Parse(Request.Form["id"]);
            string address = Request.Form["address"];
            string status = Request.Form["status"];
            Order o = _unitOfWork.OrderRepository.Find(orderId);
            o.Address = address;
            o.Status = status;
            _unitOfWork.OrderRepository.Update(o);
            _unitOfWork.SaveChange();
            return RedirectToPage();

        }
        public IActionResult OnGetExportFile()
        {
            var data = _unitOfWork.OrderRepository.GetAll().ToList(); // Lấy dữ liệu từ Backend
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            streamWriter.WriteLine("OrderId," + "UserId," + "OrderDate," + "Status," + "Address," + "Total");
            // Ghi dữ liệu vào tập tin
            foreach (var item in data)
            {
                streamWriter.WriteLine(item.OrderId + "," + item.UserId+","+item.OrderDate+","+item.Status+","+item.Address+","+item.Total);
            }
            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Trả về tập tin đã tạo
            return File(memoryStream, "text/csv", "Orders.csv");
        }
    }
}
