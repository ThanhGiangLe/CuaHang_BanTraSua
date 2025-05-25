using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class OrderDTO
    {
        [Key]
        public int OrderId { get; set; } = default;
        public int UserId { get; set; } = default;
        public DateTime OrderTime { get; set; } = default;
        public int TableId { get; set; } = default;
        public decimal TotalAmount { get; set; } = default;
        public string Status { get; set; } = string.Empty; // e.g., "Paid", "Unpaid"
        public decimal Discount { get; set; } = default;
        public decimal Tax { get; set; } = default;

        //public User User { get; set; }
        //public Table Table { get; set; }
        //public ICollection<OrderItem> OrderItems { get; set; }
    }
}
