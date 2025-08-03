using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    [Table("FoodCategorys")]
    public class FoodCategoryMdl
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Icon { get; set; } = string.Empty;
    }
}