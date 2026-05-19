using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IstockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(s => s.Comments).ThenInclude(c => c.AppUser).AsQueryable();

            if (!string.IsNullOrEmpty(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.ToUpper() == query.Symbol.ToUpper());
            }

            if (!string.IsNullOrEmpty(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.ToUpper() == query.CompanyName.ToUpper());
            }

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                stocks = query.SortBy.ToLower() switch
                {
                    "symbol" => query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol),
                    "companyname" => query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName),
                    _ => stocks
                };
            }
            else if (query.IsDescending)
            {
                stocks = stocks.OrderByDescending(s => s.Id);
            }


            var skip = (query.PageNumber - 1) * query.PageSize;
            stocks = stocks.Skip(skip).Take(query.PageSize);

            return await stocks.ToListAsync();
        }


        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.Include(s => s.Comments).ThenInclude(c => c.AppUser).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock> CreateStockAsync(CreateStockDtos stock)
        {
            var stockModel = StockMappers.ToStockFromCreateStockDtos(stock);
            _context.Stocks.Add(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockDtos updateStockDtos)
        {
            var existing = await _context.Stocks.Include(s => s.Comments).ThenInclude(c => c.AppUser).FirstOrDefaultAsync(s => s.Id == id);
            if (existing == null) return null;

            StockMappers.UpdateStockFromUpdateStockDtos(existing, updateStockDtos);

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var stock = await _context.Stocks.Include(s => s.Comments).ThenInclude(c => c.AppUser).FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null) return null;

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public Task<bool> StockExistsAsync(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public Task<bool> StockExistsAsync(string symbol)
        {
            return _context.Stocks.AnyAsync(s => s.Symbol.ToUpper() == symbol.ToUpper());
        }

        public async Task<Stock?> GetStockBySymbolAsync(string symbol)
        {
            return await _context.Stocks.Include(s => s.Comments).ThenInclude(c => c.AppUser).FirstOrDefaultAsync(s => s.Symbol.ToUpper() == symbol.ToUpper());
        }

    }
}