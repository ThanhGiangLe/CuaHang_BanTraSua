namespace testVue.ModelsRequest
{
    public class CancelAllGoodsRequestDetail
    {
        public int MaterialId { get; set; } = default;
        public DateTime ImportDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Quantity { get; set; }

    }
}
