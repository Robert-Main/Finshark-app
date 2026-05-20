using api.Dtos;
using api.Dtos.Account;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class UserMappers
    {
        public static UserResponseDto MapToUserResponseDto(AppUser user)
        {
            return new UserResponseDto
            {
                UserName = user.UserName,
                Email = user.Email,
                AppUserId = user.Id,
                Stocks = user.Portfolios
                    .Where(p => p.Stock != null)
                    .Select(p => MapStockSummary(p.Stock!))
                    .ToList(),
                Portfolios = user.Portfolios
                    .Where(p => p.Stock != null)
                    .Select(p => new PortfolioResponseDto
                    {
                        AppUserId = p.AppUserId,
                        StockId = p.StockId,
                        Symbol = p.Stock!.Symbol,
                        CompanyName = p.Stock.CompanyName
                    })
                    .ToList(),
                Comments = user.Comments
                    .Select(c =>
                    {
                        var dto = CommentMappers.MapCommentResponseDtos(c);
                        dto.CreatedBy = user.UserName ?? "Unknown";
                        return dto;
                    })
                    .ToList()
            };
        }

        private static StockResponseDtos MapStockSummary(Stock stock)
        {
            return new StockResponseDtos
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap
            };
        }
    }
}
