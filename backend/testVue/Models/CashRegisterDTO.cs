using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class CashRegisterDTO
    {
        [Key]
        public int? CashRegisterId { get; set; }
        public int? UserId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? TotalIncome { get; set; }
        public UserDTO? User { get; set; }
    }
}