using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Orders
{
    public class CartModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public ICollection<Cart> Carts { get; set; } = default!;

        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public double Total { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToPage("/Books/Index");
            }
            Carts = _unitOfWork.CartRepository.FindByUserId(HttpContext.Session.GetInt32("userid").Value);
            return Page();
        }

        public IActionResult OnGetDeleteCart(int cartId)
        {
            _unitOfWork.CartRepository.Delete(cartId);
            _unitOfWork.SaveChange();
            HttpContext.Session.SetInt32("cartcount", _unitOfWork.CartRepository.FindByUserId(HttpContext.Session.GetInt32("userid").Value).Count);
            return RedirectToPage();
        }

        public IActionResult OnGetAddCart(int bookId)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToPage("/Auth/Login");
            }
            // Kiểm tra Referer để xác định trang gọi action
            var refererUrl = Request.Headers["Referer"].ToString();
            bool returnUrl = refererUrl.Contains("Detail");
            Cart cart = _unitOfWork.CartRepository.FindByBookIdAndUserId(bookId, HttpContext.Session.GetInt32("userid").Value);
            if (cart != null)
            {
                cart.Quantity += 1;
                _unitOfWork.CartRepository.Update(cart);
                _unitOfWork.SaveChange();
            }
            else
            {
                _unitOfWork.CartRepository.Add(new Cart
                {
                    BookId = bookId,
                    Quantity = 1,
                    UserId = HttpContext.Session.GetInt32("userid").Value
                });
                _unitOfWork.SaveChange();
                HttpContext.Session.SetInt32("cartcount", _unitOfWork.CartRepository.FindByUserId(HttpContext.Session.GetInt32("userid").Value).Count);
            }
            if (returnUrl)
            {
                return RedirectToPage("/Books/Detail", new { id = bookId });
            }
            return RedirectToPage("/Books/Search");

        }

        public IActionResult OnGetAddQuantity(int cartId)
        {
            Cart cart = _unitOfWork.CartRepository.FindCartByIdWithNav(cartId);
            if (cart != null)
            {
                cart.Quantity += 1;
                _unitOfWork.CartRepository.Update(cart);
                _unitOfWork.SaveChange();
                return new JsonResult(cart);
            }

            return Page();
        }

        public IActionResult OnGetMinusQuantity(int cartId)
        {
            Cart cart = _unitOfWork.CartRepository.FindCartByIdWithNav(cartId);
            if (cart != null)
            {
                if (cart.Quantity == 1)
                {
                    _unitOfWork.CartRepository.Delete(cart);
                    _unitOfWork.SaveChange();
                    HttpContext.Session.SetInt32("cartcount", _unitOfWork.CartRepository.FindByUserId(HttpContext.Session.GetInt32("userid").Value).Count);
                    cart.Quantity = 0;
                    return new JsonResult(cart);
                }
                else
                {
                    cart.Quantity -= 1;
                    _unitOfWork.CartRepository.Update(cart);
                    _unitOfWork.SaveChange();
                    return new JsonResult(cart);
                }
            }

            return Page();
        }
        public IActionResult OnPost()
        {
            Carts = _unitOfWork.CartRepository.FindByUserId(HttpContext.Session.GetInt32("userid").Value);
            Order order = new Order
            {
                UserId = HttpContext.Session.GetInt32("userid").Value,
                Address = Address,
                OrderDate = DateTime.Now,
                Total = Total,
                Status = "Pending"
            };
            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.SaveChange();
            foreach (var item in Carts)
            {
                _unitOfWork.OrderDetailRepository.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    Quantity = item.Quantity
                });
                _unitOfWork.SaveChange();

            }
            _unitOfWork.CartRepository.DeleteAllByUserId(HttpContext.Session.GetInt32("userid").Value);
            HttpContext.Session.SetInt32("cartcount", 0);
            _unitOfWork.SaveChange();

            return RedirectToPage("/Orders/CheckoutSuccess");
        }
    }
}
