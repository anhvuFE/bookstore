using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using LibraryCore.Models;
using Microsoft.AspNetCore.Http;

namespace Library.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IEmailSender _emailSender;

        public RegisterModel(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public bool check { get; set; } = false;
        public string error { get; set; }
        public IActionResult OnPost(string fullName, string account, string password, string cfPassword, string toEmail)
        {

            if (password != cfPassword)
            {
                error = "Password and confirmpassword not match";
                return Page();
            }
            else if (_unitOfWork.UserRepository.FindUserByName(account) != null)
            {
                error = "User name already exsist please input another";
                return Page();
            }


            string otp = _emailSender.GetOTP();
            string mailBody = $"<h4>Thank you for registering an account on our website.</h4>" +
                $"<p>Here is your OTP: {otp}</p>" +
                $"<img src='https://th.bing.com/th/id/OIP.Sbr17XYoywddAYpXUY_f-gHaEm?w=274&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7'/>";
            bool sendMailCheck = _emailSender.SendEmail(toEmail, "Register OTP", mailBody);

            User u = new User { Fullname = fullName, Password = password, Username = account, Mail = toEmail, UserRole = "user", Status = false };

            string idRegister = _unitOfWork.UserRepository.AddUser(u);
            check = true;
            if (sendMailCheck)
            {
                HttpContext.Session.SetString("otp", otp);
                HttpContext.Session.SetString("idRegister", idRegister);
                return RedirectToPage("/Auth/Otp");
            }

            error = "Email not exsist";




            return Page();
        }



    }
}
