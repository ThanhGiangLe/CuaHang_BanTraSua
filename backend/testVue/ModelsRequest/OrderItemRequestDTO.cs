using System.ComponentModel.DataAnnotations;

namespace testVue.ModelsRequest
{
    public class OrderItemRequestDTO
    {
        [Key]
        public int OrderId { get; set; } = default;
        public int FoodItemId { get; set; } = default;
        public string? FoodName { get; set; } = string.Empty;
        public int Quantity { get; set; } = default;
        public decimal Price { get; set; } = default;
        public int IsMainItem { get; set; } = default;
        public string Unit { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public int CategoryId { get; set; } = default;
        public DateTime? OrderTime { get; set; }


    }
}
