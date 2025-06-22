namespace testVue.ModelsRequest
{
    public class OrderDetailShowViewReportDTO
    { 
        public int OrderId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime OrderTime { get; set; }
        public string TableName { get; set; } = string.Empty;
        public decimal ReceivedAmount { get; set; } = default;
        public decimal ReturnedAmount { get; set; } = default;
        public decimal TotalAmount { get; set; } = default;
        public decimal Discount { get; set; } = default;
        public string PaymentMethod {  get; set; } = string.Empty;
    }
}
