using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
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

            var stockData = StockMappers.MapStockToStockResponseDtos(stock );
            return Ok(new
            {
                success = true,
                message = "Stock retrieved successfully",
                data = stockData
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] Stock stock)
        {
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();
            var stockData = StockMappers.MapStockToStockResponseDtos(stock);
            return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stockData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}