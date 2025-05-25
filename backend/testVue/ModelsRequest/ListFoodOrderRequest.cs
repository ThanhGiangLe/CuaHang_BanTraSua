using System.Text.Json.Serialization;

namespace testVue.ModelsRequest
{
    public class ListFoodOrderRequest
    {
        public List<FoodOrderRequest> Items { get; set; }
    }
    public class FoodOrderRequest
    {
        [JsonPropertyName("FoodItemId")]
        public int FoodItemId { get; set; } = default;

        [JsonPropertyName("FoodName")]
        public string FoodName { get; set; } = string.Empty;

        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; } = default;

        [JsonPropertyName("ListAdditionalFood")]
        public List<AdditionalFood> ListAdditionalFood { get; set; } = new();
    }
    public class AdditionalFood
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = default;

        [JsonPropertyName("foodName")]
        public string FoodName { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = default;
    }
}
