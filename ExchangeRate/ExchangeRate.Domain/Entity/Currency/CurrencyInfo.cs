using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Domain.Entity.Currency
{
    public class CurrencyInfo
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public DateTime Date { get; set; } 
    }
}
