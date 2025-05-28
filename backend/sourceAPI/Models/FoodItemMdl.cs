using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    [Table("FoodItems")]
    public class FoodItemMdl
    {
        [Key]
        public int FoodItemId { get; set; } = default;
        [Required]
        public string FoodName { get; set; } = string.Empty;
        [Required]
        public decimal PriceListed { get; set; } = default;
        public decimal? PriceCustom { get; set; } = default;
        public string ImageUrl { get; set; } = string.Empty;
        public string Unit { get; set; } = "Ly";
        public int CategoryId { get; set; } = default;
        [Required]
        public string Status { get; set; } = "Available";
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; } = string.Empty;
        public string UpdateBy { get; set; } = string.Empty;
        public int IsMain { get; set; } = default;
    }
}