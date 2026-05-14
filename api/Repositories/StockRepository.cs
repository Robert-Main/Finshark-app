using api.Data;
using api.Dtos.Stock;
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

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _context.Stocks.Include(s => s.Comments).ToListAsync();
        }


        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
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
            var existing = await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
            if (existing == null) return null;

            StockMappers.UpdateStockFromUpdateStockDtos(existing, updateStockDtos);

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var stock = await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
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
            return await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Symbol.ToUpper() == symbol.ToUpper());
        }
    }
}