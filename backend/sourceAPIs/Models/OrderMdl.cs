using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class OrderMdl
    {
        [Key]
        public int OrderId { get; set; } = default;
        public int UserId { get; set; } = default;
        public DateTime OrderTime { get; set; } = default;
        public int? TableId { get; set; }
        public decimal TotalAmount { get; set; } = default;
        public decimal TotalResult { get; set; } = default;
        public string Status { get; set; } = string.Empty; 
        public decimal Discount { get; set; } = default;
        public decimal Tax { get; set; } = default;
        public decimal ReceivedAmount { get; set; } = default;
        public decimal ReturnedAmount { get; set; } = default;
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
