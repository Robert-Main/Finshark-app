using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.interfaces
{
    public interface IPortfolioRepository
    {
        public Task<List<Stock>> GetUserPortfolio(AppUser user);

        public Task<Portfolio> CreateAsync(Portfolio portfolio);

        //delete from portfolio
        public Task<Portfolio> DeleteFromPortfolioAsync(AppUser user, string symbol);
    }
}