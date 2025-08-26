using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Domain.Entity.Currency.Request
{
    public class CurrencyRequest
    {
        public string? Code { get; set; }
        public string? CodeIn { get; set; }
        public string DateOfCurrency { get; set; }

    }
}
