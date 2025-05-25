namespace testVue.Models.Inventory.Inventory
{
    public class CancelAllGoodsRequest
    {
        public int MaterialId { get; set; } = default;
        public double Quantity { get; set; } = default;
    }
}
