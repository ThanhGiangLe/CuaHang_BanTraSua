using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testVue.Models
{
    public class WarehouseHistoryDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WarehouseHistoryId { get; set; } = default;

        [Required]
        public int MaterialId { get; set; } = default;

        [ForeignKey("MaterialId")]
        public MaterialDTO? Material { get; set; }

        public DateTime? ImportDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Unit { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ImportPrice { get; set; } = default; // Giá nhập

        [Required]
        [Range(0, double.MaxValue)]
        public double Quantity { get; set; } // Số lượng nhập hàng

        [Required]
        [Range(0, double.MaxValue)]
        public double CurrentQuantity { get; set; } // Số lượng nhập hàng hiện còn lại

        [Required]
        [Range(0, int.MaxValue)]
        public int Using { get; set; } // Tình trạng có đang sử dụng không
    }
}
