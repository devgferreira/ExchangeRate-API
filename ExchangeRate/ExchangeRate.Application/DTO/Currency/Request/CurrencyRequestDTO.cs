using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.DTO.Currency.Request
{
    public class CurrencyRequestDTO
    {
        public string? Code { get; set; }
        public string? CodeIn { get; set; }
    }
}
