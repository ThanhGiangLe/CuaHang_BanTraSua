namespace testVue.ModelsRequest
{
    public class OrderDetailShowViewReportDTO
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime OrderTime { get; set; }
        public string TableName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; } = default;
        public decimal Discount { get; set; } = default;
        public decimal Tax { get; set; } = default;
    }
}
