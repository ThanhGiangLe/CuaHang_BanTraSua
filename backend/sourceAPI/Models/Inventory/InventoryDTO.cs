//using System.ComponentModel.DataAnnotations;

//namespace testVue.Models.Inventory
//{
//    public class InventoryDTO
//    {
//        [Key]
//        public int InventoryId { get; set; } = default;
//        public string ItemName { get; set; } = string.Empty;
//        public int Quantity { get; set; } = default;
//        public int MinQuantity { get; set; } = default;
//        public string Unit { get; set; } = string.Empty;
//        public string ItemType { get; set; } = string.Empty; // e.g., "Raw Material", "Packaged Goods"

//        public ICollection<InventoryLogDTO>? InventoryLogs { get; set; } = default;
//    }
//}