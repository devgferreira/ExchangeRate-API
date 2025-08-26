using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.DTO.Currency
{
    public class CurrencyDTO
    {
        public string Code { get; set; }
        public string Codein { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public DateTime DateOfCurrency { get; set; }
        public DateTime CreatedAT { get; set; }
    }
}
