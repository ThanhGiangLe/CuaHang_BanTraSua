namespace testVue.ModelsRequest
{
    public class CashRegisterUpdateTotalAmountRequest
    {
        public int? UserId { get; set; }
        public decimal TotalAmount { get; set; } = default;
    }
}
