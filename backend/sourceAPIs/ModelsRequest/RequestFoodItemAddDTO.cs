using System.ComponentModel.DataAnnotations;

namespace testVue.Models.Food
{
    public class RequestFoodItemAddDTO
    {
        public string FoodName { get; set; } = string.Empty;
        public decimal PriceListed { get; set; } = default;
        public decimal? PriceCustom { get; set; } = default;
        public string ImageUrl { get; set; } = string.Empty;
        public string Unit { get; set; } = "phần";
        public int CategoryId { get; set; } = default;
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
        public int IsMain { get; set; } = default;
    }
}
