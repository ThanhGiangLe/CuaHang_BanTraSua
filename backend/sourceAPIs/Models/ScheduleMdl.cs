using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class ScheduleMdl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; } = default;
        public int UserId { get; set; } = default;
        public int Date {  get; set; }
        public string ShiftId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy {  get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
        public decimal CashAmount { get; set; }
        public decimal BankAmount { get; set; }
        public decimal ClosingCashAmount { get; set; }
    }
}
