using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testVue.Models
{
    [Table("AdditionalFood")]
    public class AdditionalFoodDTO
    {
        [Key]
        public int Id { get; set; } = default;

        public string FoodName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal Price { get; set; } = decimal.Zero;

        public int CategoryId { get; set; } = default;

        public int Quantity { get; set; } = default;

    }
}