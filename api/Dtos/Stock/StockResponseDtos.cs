using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos
{
    public class StockResponseDtos
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; } = 0;
        public decimal LastDiv { get; set; } = 0;

        public string Industry { get; set; } = string.Empty;
        public decimal MarketCap { get; set; } = 0;

    }
}