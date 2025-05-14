using LibraryCore.Models;
using LibraryCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;

namespace Library.Pages.Auth
{
    public class RePasswordModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IEmailSender _emailSender;

        public RePasswordModel(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public bool check { get; set; } = false;
        public string error { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string account)
        {
            string otp = _emailSender.GetOTP();
            User user = _unitOfWork.UserRepository.FindUserByName(account);

            string mailBody = $"<h4>Thank you for used an account on our website.</h4>" +
                $"<p>Here is your new password: {otp}</p>" +
                $"<img src='https://th.bing.com/th/id/OIP.Sbr17XYoywddAYpXUY_f-gHaEm?w=274&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7'/>";
            bool sendMailCheck = _emailSender.SendEmail(user.Mail, "Rest password OTP", mailBody);

            check = true;
            if (sendMailCheck)
            {
                user.Status = true;
                user.Password = otp;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.SaveChange();
                HttpContext.Session.SetString("message", "New password was send to your email");
                return RedirectToPage("/Auth/Login");
            }

            error = "Email not exsist";

            return Page();
        }
    }
}
