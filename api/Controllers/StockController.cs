using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly ILogger<StockController> _logger;
        private readonly ApplicationDBContext _context;

        public StockController(ILogger<StockController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stockData = stocks.Select(s => StockMappers.MapStockToStockResponseDtos(s)).ToList();

            return Ok(new
            {
                success = true,
                message = "Stocks retrieved successfully",
                data = stockData
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStock(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);


            if (stock == null || stock.Id != id)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Stock with that id not found"
                });

            }

            var stockData = StockMappers.MapStockToStockResponseDtos(stock);
            return Ok(new
            {
                success = true,
                message = "Stock retrieved successfully",
                data = stockData
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDtos stock)
        {
            if (stock == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Stock data is required"
                });
            }

            var stockModel = StockMappers.ToStockFromCreateStockDtos(stock);
            _context.Stocks.Add(stockModel);
            await _context.SaveChangesAsync();
            var stockData = StockMappers.MapStockToStockResponseDtos(stockModel);

            return CreatedAtAction(nameof(GetStock), new { id = stockModel.Id }, new
            {
                success = true,
                message = "Stock created successfully",
                data = stockData
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDtos stock)
        {
            if (stock == null)
                return BadRequest(new { success = false, message = "Stock data is required" });

            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
                return NotFound(new
                {
                    success = false,
                    message = "Stock with that id not found"
                });

            StockMappers.UpdateStockFromUpdateStockDtos(stockModel, stock);

            await _context.SaveChangesAsync();

            var stockData = StockMappers.MapStockToStockResponseDtos(stockModel);

            return Ok(new
            {
                success = true,
                message = "Stock updated successfully",
                data = stockData
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
                return NotFound(new
                {
                    success = false,
                    message = "Stock with that id not found"
                });

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Stock deleted successfully"
            });
        }

    }
}