using System.ComponentModel.DataAnnotations;

namespace LibraryCore.Models
{
    public partial class User
    {
        public User()
        {

        }

        public int UserId { get; set; }
        public string Fullname { get; set; } = null!;
        public string Username { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string UserRole { get; set; } = null!;
        public bool Status { get; set; }
    }
}
