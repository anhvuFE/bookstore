using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages.Auth
{
    public class ProfileModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string status { get; set; } = "1";
        public User user { get; set; }
        public string mess { get; set; }
        public IActionResult OnGet(string st)
        {
            if (st != null)
            {
                status = st;
            }

            int id = (int)HttpContext.Session.GetInt32("userid");
            user = _unitOfWork.UserRepository.Find(id);

            return Page();
        }

        public IActionResult OnPost(string fullname, string password, string cfPassword, string st)
        {
            int id = (int)HttpContext.Session.GetInt32("userid");
            user = _unitOfWork.UserRepository.Find(id);
            if (st == "1")
            {
                status = st;
                user.Fullname = fullname;
                mess = "Update infomation success";
            }
            else if (st == "2")
            {
                status = st;
                if (password == cfPassword)
                {
                    user.Password = password;
                    mess = "Update infomation success";
                }
                else
                {
                    mess = "Password and re-password not match";
                }
            }
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.SaveChange();



            return Page();
        }
    }
}
