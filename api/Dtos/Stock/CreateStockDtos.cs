using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CreateStockDtos
    {
        [Required(ErrorMessage = "Symbol is required.")]
        [MinLength(1, ErrorMessage = "Symbol cannot be empty.")]
        [MaxLength(10, ErrorMessage = "Symbol cannot be longer than 10 characters.")]
        public string Symbol { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company Name is required.")]
        [MinLength(2, ErrorMessage = "Company Name cannot be less than 2 characters.")]
        [MaxLength(100, ErrorMessage = "Company Name cannot be longer than 100 characters.")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purchase price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be a positive number.")]
        public decimal Purchase { get; set; } = 0;

        [Required(ErrorMessage = "Last dividend is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Last dividend must be a non-negative number.")]
        public decimal LastDiv { get; set; } = 0;

        [Required(ErrorMessage = "Industry is required.")]
        [MinLength(2, ErrorMessage = "Industry cannot be less than 2 characters.")]
        [MaxLength(100, ErrorMessage = "Industry cannot be longer than 100 characters.")]
        public string Industry { get; set; } = string.Empty;

        [Required(ErrorMessage = "Market capitalization is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Market capitalization must be a non-negative number.")]
        public decimal MarketCap { get; set; } = 0;
    }
}