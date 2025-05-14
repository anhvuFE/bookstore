using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCore.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

        public string Address { get; set; }

        public double Total { get; set; }


        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
