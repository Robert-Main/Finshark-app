using api.Dtos.Stock;
using api.Helpers;
using api.interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IstockRepository _stockRepository;

        public StockController(IstockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid model state"
                });

            var stocks = await _stockRepository.GetAllStocksAsync(query);
            var stockData = stocks.Select(s => StockMappers.MapStockToStockResponseDtos(s)).ToList();
            return Ok(new { success = true, message = "Stocks retrieved successfully", data = stockData });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStock([FromRoute] int id)
        {
            var stock = await _stockRepository.GetStockByIdAsync(id);
            if (stock == null)
                return NotFound(new { success = false, message = "Stock with that id not found" });

            return Ok(new { success = true, message = "Stock retrieved successfully", data = StockMappers.MapStockToStockResponseDtos(stock) });
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDtos stock)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid model state" });

            if (stock == null)
                return BadRequest(new { success = false, message = "Stock data is required" });

            var stockModel = await _stockRepository.CreateStockAsync(stock); // ✅ pass DTO directly
            return CreatedAtAction(nameof(GetStock), new { id = stockModel.Id }, new { success = true, message = "Stock created successfully", data = StockMappers.MapStockToStockResponseDtos(stockModel) });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDtos stock)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid model state" });

            if (stock == null)
                return BadRequest(new { success = false, message = "Stock data is required" });

            var stockModel = await _stockRepository.UpdateStockAsync(id, stock); // ✅ pass DTO directly
            if (stockModel == null)
                return NotFound(new { success = false, message = "Stock with that id not found" });

            return Ok(new { success = true, message = "Stock updated successfully", data = StockMappers.MapStockToStockResponseDtos(stockModel) });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var stock = await _stockRepository.DeleteStockAsync(id);
            if (stock == null)
                return NotFound(new { success = false, message = "Stock with that id not found" });

            return Ok(new { success = true, message = "Stock deleted successfully" });
        }
    }
}