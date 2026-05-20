using api.Dtos;
using api.Dtos.Stock;

namespace api.Dtos.Account
{
    public class UserResponseDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? AppUserId { get; set; }
        public List<StockResponseDtos> Stocks { get; set; } = new();
        public List<PortfolioResponseDto> Portfolios { get; set; } = new();
        public List<CommentResponseDtos> Comments { get; set; } = new();

    }
}
