using System.ComponentModel.DataAnnotations;

namespace testVue.ModelsRequest
{
    public class RequestUpdateFoodItemDTO
    {
        [Key]
        public int FoodItemId { get; set; } = default;
        public string FoodName { get; set; } = string.Empty;
        public decimal PriceListed { get; set; } = default;
        public decimal? PriceCustom { get; set; } = default;
        public string ImageUrl { get; set; } = string.Empty;    
        public string Unit { get; set; } = "Ly";
        public int? CategoryId { get; set; } = default;
        public string Status { get; set; } = string.Empty;
        public string? UpdateBy { get; set; } = string.Empty;
        public int IsMain { get; set; } = default;
    }
}
