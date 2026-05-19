using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();

            return portfolio;
        }

        public async Task<Portfolio> DeleteFromPortfolioAsync(AppUser user, string symbol)
        {
            var portfolioToDelete = await _context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == user.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());
            if (portfolioToDelete == null)
            {
                return null;
            }
            _context.Portfolios.Remove(portfolioToDelete);
            await _context.SaveChangesAsync();
            return await Task.FromResult(portfolioToDelete);
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            var portfolio = _context.Portfolios.Where(p => p.AppUserId == user.Id).Select(p => new Stock
            {
                Id = p.Stock.Id,
                CompanyName = p.Stock.CompanyName,
                Symbol = p.Stock.Symbol,
                Purchase = p.Stock.Purchase,
                LastDiv = p.Stock.LastDiv,
                Industry = p.Stock.Industry,
                MarketCap = p.Stock.MarketCap,
            }).ToListAsync();
            return await portfolio;
        }


    }
}