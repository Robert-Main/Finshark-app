using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.interfaces;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortfolioController : Controller
    {
        private readonly ILogger<PortfolioController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly PortfolioRepository _portfolioRepository;
        private readonly IstockRepository _stockRepository;

        public PortfolioController(ILogger<PortfolioController> logger, UserManager<AppUser> userManager, PortfolioRepository portfolioRepository, IstockRepository stockRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _portfolioRepository = portfolioRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsersPortfolio()
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "User not found"
                });
            }

            var portfolio = await _portfolioRepository.GetUserPortfolio(user);
            return Ok(new
            {
                sucesses = true,
                portfolio
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToPortfolio(string symbol)
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "User not found"
                });
            }

            var stock = await _stockRepository.GetStockBySymbolAsync(symbol);
            if (stock == null)
            {
                return NotFound(new
                {
                    message = "Stock not found"
                });
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(user);
            if (userPortfolio.Any(s => s.Symbol.ToLower() == stock.Symbol.ToLower()))
            {
                return BadRequest(new
                {
                    message = "Stock already in portfolio"
                });
            }

            var portfolioEntry = new Portfolio
            {
                AppUserId = user.Id,
                StockId = stock.Id
            };
            await _portfolioRepository.CreateAsync(portfolioEntry);

            if (portfolioEntry == null)
            {
                return BadRequest(new
                {
                    message = "Failed to add stock to portfolio"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Stock added to portfolio"
            });
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveFromPortfolio(string symbol)
        {
            var username = User.GetUserName();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "User not found"
                });
            }

            var stock = await _stockRepository.GetStockBySymbolAsync(symbol);
            if (stock == null)
            {
                return NotFound(new
                {
                    message = "Stock not found"
                });
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(user);
            var filteredPortfolio = userPortfolio.Where(s => s.Symbol.ToLower() == stock.Symbol.ToLower()).ToList();

            if (filteredPortfolio.Count() == 1)
            {
                await _portfolioRepository.DeleteFromPortfolioAsync(user, symbol);
            }
            else
            {
                return NotFound(new
                {
                    message = "Stock not found in user's portfolio"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Stock removed from portfolio"
            });
        }
    }
}