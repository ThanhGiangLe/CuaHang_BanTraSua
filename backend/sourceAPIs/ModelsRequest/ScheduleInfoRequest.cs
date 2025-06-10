namespace sourceAPI.ModelsRequest
{
    public class ScheduleInfoRequest
    {
        public int UserId { get; set; }
        public string UpdateBy { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<Shift> Schedules { get; set; } = new List<Shift>();
    }
    public class Shift
    {
        public int Day { get; set; }
        public string ShiftCode { get; set; }
    }
}
