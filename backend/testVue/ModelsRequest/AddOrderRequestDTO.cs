using System.ComponentModel.DataAnnotations;

namespace testVue.ModelsRequest
{
    public class AddOrderRequestDTO
    {
        [Key]
        public int UserId { get; set; } = default;
        public DateTime OrderTime { get; set; }
        public int TableId { get; set; } = default;
        public decimal TotalAmount { get; set; } = default;
        public string Status { get; set; } = string.Empty; // e.g., "Paid", "Unpaid"
        public decimal Discount { get; set; } = default;
        public decimal Tax { get; set; } = default;
        //public List<OrderItemRequest> Items { get; set; }
    }
}
