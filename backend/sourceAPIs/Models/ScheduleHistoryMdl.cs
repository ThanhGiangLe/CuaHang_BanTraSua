using System.ComponentModel.DataAnnotations;

namespace sourceAPI.Models
{
    public class ScheduleHistoryMdl
    {
        [Key]
        public int ScheduleHistoryId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string OldShiftId { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
