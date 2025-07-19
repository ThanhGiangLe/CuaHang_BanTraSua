using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sourceAPI.Models
{
    public class ScheduleShiftMdl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleShiftId { get; set; }
        public int ScheduleId { get; set; } = default;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal OpeningCashAmount { get; set; }
        public decimal ReceivedTotalAmount { get; set; }
        public decimal ReturnedTotalAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public string? AdjustmentReason { get; set; } = string.Empty;
        public decimal ClosingCashAmount { get; set; }
    }
}
