using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class OrderItemDTO
    {
        [Key]
        public int OrderItemId { get; set; } = default;
        public int OrderId { get; set; } = default;
        public int FoodItemId { get; set; } = default;
        public int Quantity { get; set; } = default;
        public decimal Price { get; set; } = default;
        //public int? CustomItemId { get; set; }

        //public Order Order { get; set; }
        //public FoodItem FoodItem { get; set; }
        //public CustomizableItem CustomItem { get; set; }
        public string FoodName { get; internal set; } = string.Empty;
        public int IsMainItem { get; internal set; } = default;
        public string Unit { get; internal set; } = string.Empty;
        public string Note { get; internal set; } = string.Empty;
        public int CategoryId { get; set; } = default;

        public DateTime? OrderTime { get; set; }

    }

}