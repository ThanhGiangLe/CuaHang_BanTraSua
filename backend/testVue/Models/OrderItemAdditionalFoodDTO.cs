namespace testVue.Models
{
    public class OrderItemAdditionalFoodDTO
    {
        public int OrderItemAdditionalFoodId { get; set; } = default;
        public int OrderItemId { get; set; } = default;  // Liên kết với OrderItem
        public int AdditionalFoodId { get; set; } = default;  // Liên kết với món ăn bổ sung (FoodItem)
        public decimal Price { get; set; } = default;  // Giá món ăn bổ sung
        public string Unit { get; set; } = string.Empty;
        public OrderItemDTO? OrderItem { get; set; }  // Mối quan hệ với OrderItem
        public AdditionalFoodDTO? AdditionalFood { get; set; }  // Mối quan hệ với món ăn bổ sung
    }
}
