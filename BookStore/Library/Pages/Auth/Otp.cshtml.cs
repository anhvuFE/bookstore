using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Pages
{
    public class OtpModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public OtpModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public string error { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnPost(string otpInput)
        {
            string otp = HttpContext.Session.GetString("otp");
            string idRegister = HttpContext.Session.GetString("idRegister");
            string account = HttpContext.Session.GetString("account");

            if (otpInput != null && otpInput == otp)
            {
                if (idRegister != null)
                {

                    User u = _unitOfWork.UserRepository.Find(int.Parse(idRegister));
                    u.Status = true;
                    _unitOfWork.UserRepository.Update(u);
                    _unitOfWork.SaveChange();
                    HttpContext.Session.SetString("message", "Register sucessfully");
                    return RedirectToPage("/Auth/Login");
                }
                /*if (account != null)
                {
                    User u = _unitOfWork.UserRepository.FindUserByName(account);
                    u.Status = true;
                    u.Password = otp;

                    _unitOfWork.UserRepository.Update(u);
                    _unitOfWork.SaveChange();
                    HttpContext.Session.SetString("message", "New password was send to your email");
                    return RedirectToPage("/Login");
                }*/
            }

            error = "Wrong otp";
            return Page();

        }

    }
}
