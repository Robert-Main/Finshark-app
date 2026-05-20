namespace api.Dtos.Account
{
    public class PortfolioResponseDto
    {
        public string? AppUserId { get; set; }
        public int StockId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
    }
}
