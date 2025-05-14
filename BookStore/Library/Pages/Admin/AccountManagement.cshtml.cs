using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    public class AccountManagementModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountManagementModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public User User { get; set; }
        public IList<User> Users { get; set; }

        public void OnGet()
        {
            Users = _unitOfWork.UserRepository.GetAll();

        }

        public IActionResult OnPostEditUser()
        {
            int userId = int.Parse(Request.Form["id"]);
            string role = Request.Form["role"];
            string status = Request.Form["status"];
            User u = _unitOfWork.UserRepository.Find(userId);
            u.UserRole = role;
            u.Status = bool.Parse(status);
            _unitOfWork.UserRepository.Update(u);
            _unitOfWork.SaveChange();
            return RedirectToPage();
        }
        public IActionResult OnPostAddUser()
        {
            _unitOfWork.UserRepository.Add(User);
            _unitOfWork.SaveChange();
            return RedirectToPage();
        }
    }
}
