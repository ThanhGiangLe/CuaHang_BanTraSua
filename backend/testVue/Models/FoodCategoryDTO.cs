using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    [Table("FoodCategory")]
    public class FoodCategoryDTO
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;
    }
}