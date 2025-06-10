namespace sourceAPI.ModelsRequest
{
    public class GetTotalSalesRequest
    {
        public DateTime Day { get; set; } = default(DateTime);
        public int UserId { get; set; } = 0;
    }
}
