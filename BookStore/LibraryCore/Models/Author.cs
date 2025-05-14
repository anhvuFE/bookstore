using System;
using System.Collections.Generic;

namespace LibraryCore.Models
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int AuthorId { get; set; }
        public string Fullname { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Infomation { get; set; } = null!;
        public string BriefInfomaton { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
