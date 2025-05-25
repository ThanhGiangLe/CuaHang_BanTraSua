namespace testVue.ModelsRequest
{
    public class UpdateQuantityMaterialDTO
    {
        public int MaterialId { get; set; } = default; // ID của nguyên liệu cần cập nhật
        public double AddedQuantity { get; set; } = default; // Số lượng cần cộng thêm
    }
}
