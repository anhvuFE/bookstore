
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace LibraryCore.UnitOfWork
{
    public class EmailSender : IEmailSender
    {
        public bool SendEmail(string toEmail, string subject, string mess)
        {
            var FromEmail = "anhvxhe161740@fpt.edu.vn";
            var pass = "arrw lbcr pgmb kycg";

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(FromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));

            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = mess };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(FromEmail, pass);

            // Check if the `toEmail` address is valid before sending the email
            try
            {
                var mailAddress = new MailAddress(toEmail);
                smtp.Send(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }

        public string GetOTP()
        {
            Random random = new Random();
            string randomNumber = random.Next(100000, 999999).ToString(); // Generate a random number between 100000 and 999999

            return randomNumber;
        }

    }
}
