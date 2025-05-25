using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; } = default;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(12)]
        public string Phone { get; set; } = string.Empty; 

        [Required]
        public int MembershipLevel { get; set; } = default;

        [Required]
        public int Point { get; set; } = default;
    }
}
