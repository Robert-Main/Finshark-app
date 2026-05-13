using api.Dtos;
using api.Dtos.Stock;
using api.Models;

namespace api.interfaces
{
    public interface IstockRepository
    {
        Task<List<Stock>> GetAllStocksAsync();
        Task<Stock?> GetStockByIdAsync(int id);
        Task<Stock> CreateStockAsync(CreateStockDtos stock);
        Task<Stock?> UpdateStockAsync(int id, UpdateStockDtos updateStockDtos); 
        Task<Stock?> DeleteStockAsync(int id);
    }
}