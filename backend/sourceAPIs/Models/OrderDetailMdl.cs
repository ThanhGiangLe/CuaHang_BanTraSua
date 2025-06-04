using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class OrderDetailMdl
    {
        [Key]
        public int OrderDetailId { get; set; } = default;
        public int OrderId { get; set; } = default;
        public int FoodItemId { get; set; } = default;
        public int Quantity { get; set; } = default;
        public decimal Price { get; set; } = default;
        public int IsMainItem { get; internal set; } = default;
        public string Unit { get; internal set; } = string.Empty;
        public string Note { get; internal set; } = string.Empty;
        public int CategoryId { get; set; } = default;  
        public DateTime? OrderTime { get; set; }
    }

}