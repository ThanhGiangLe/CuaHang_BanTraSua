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
        public DateTime? Date {  get; set; }
        public int ShiftId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreateBy {  get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
        public decimal OpeningCashAmount { get; set; }
        public decimal CloseingCashAmount { get; set; }
    }
}
