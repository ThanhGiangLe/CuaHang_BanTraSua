namespace sourceAPI.ModelsRequest
{
    public class CloseShiftAmountRequest
    {
        public int UserId { get; set; }
        public decimal ClosingCashAmount { get; set; } = 0;
        public decimal AdjustmentAmount { get; set; } = 0;
        public string? AdjustmentReason { get; set; } = string.Empty;
    }
}
