namespace sourceAPI.ModelsRequest
{
    public class SwapScheduleShiftRequest
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
