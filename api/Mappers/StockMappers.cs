using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public class StockMappers
    {
        public static StockResponseDtos MapStockToStockResponseDtos(Stock stock)
        {
            return new StockResponseDtos
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                Comments = stock.Comments.Select(c => CommentMappers.MapCommentResponseDtos(c)).ToList()
            };
        }
        public static Stock ToStockFromCreateStockDtos(CreateStockDtos createStockDtos)
        {
            return new Stock
            {
                Symbol = createStockDtos.Symbol,
                CompanyName = createStockDtos.CompanyName,
                Purchase = createStockDtos.Purchase,
                LastDiv = createStockDtos.LastDiv,
                Industry = createStockDtos.Industry,
                MarketCap = createStockDtos.MarketCap
            };
        }

        public static void UpdateStockFromUpdateStockDtos(Stock stock, UpdateStockDtos updateStockDtos)
        {
            stock.Symbol = updateStockDtos.Symbol;
            stock.CompanyName = updateStockDtos.CompanyName;
            stock.Purchase = updateStockDtos.Purchase;
            stock.LastDiv = updateStockDtos.LastDiv;
            stock.Industry = updateStockDtos.Industry;
            stock.MarketCap = updateStockDtos.MarketCap;
        }
    }

}