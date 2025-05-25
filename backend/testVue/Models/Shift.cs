using System.ComponentModel.DataAnnotations;

namespace testVue.Models
{
    public class Shift
    {
        [Key]
        public string? ShiftId { get; set; }
        public string? ShiftName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
    }
}
