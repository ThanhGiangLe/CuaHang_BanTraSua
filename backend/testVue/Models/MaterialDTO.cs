using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class MaterialDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaterialId { get; set; } = default; // Mã nguyên liệu (auto-generated ID)

        [Required]
        [MaxLength(100)]
        public string MaterialName { get; set; } = string.Empty;// Tên nguyên liệu

        [MaxLength(50)]
        public string MaterialType { get; set; } = string.Empty; // Loại nguyên liệu

        [Required]
        [Range(0, double.MaxValue)]
        public double Quantity { get; set; } = default; // Lượng (Số lượng hiện tại)

        [Required]
        [Range(0, double.MaxValue)]
        public double MinQuantity { get; set; } = default; // Số lượng tối thiểu

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; } = string.Empty; // Đơn vị tính

        [Required]
        public DateTime? ImportDate { get; set; } // Ngày nhập

        public DateTime? ExpirationDate { get; set; } // Ngày hết hạn (nullable)

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ImportPrice { get; set; } = default; // Giá nhập

    }
}
