using ExchangeRate.Domain.Entity.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Domain.Interface
{
    public interface ICurrencyRepository
    {
        Task<bool> CreateCurrency(CurrencyInfo currencyInfo);
    }
}
