namespace testVue.ModelsRequest
{
    public class ImportWarehouseRequest
    {
        public int MaterialId { get; set; } = default;

        public string Unit { get; set; } = string.Empty;

        public decimal ImportPrice { get; set; } = default;

        public double Quantity { get; set; }
    }
}
