using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    [Table("FoodItem")]
    public class FoodItemDTO
    {
        [Key]
        public int FoodItemId { get; set; } = default;

        [Required]
        public string FoodName { get; set; } = string.Empty;

        [Required]
        public decimal PriceListed { get; set; } = default;

        public decimal? PriceCustom { get; set; } = default;

        public string ImageUrl { get; set; } = string.Empty;

        public string Unit { get; set; } = "phần";

        public int CategoryId { get; set; } = default;

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [Required]
        public string Status { get; set; } = "available";
    }
}