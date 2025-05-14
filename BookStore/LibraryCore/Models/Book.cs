namespace LibraryCore.Models
{
    public partial class Book
    {
        public Book()
        {
            //Authors = new HashSet<Author>();
        }

        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public string? Publisher { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int Quantity { get; set; }
        public string? Image { get; set; }
        public double? Rate { get; set; }
        public bool? Status { get; set; }
        public string? Introduction { get; set; }
        public string? Description { get; set; }

        public double? Price { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Author Author { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }


    }
}
