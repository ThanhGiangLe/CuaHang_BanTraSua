using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class ShiftMdl
    {
        [Key]
        public string? ShiftId { get; set; } = string.Empty;
        public string? ShiftName { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}
