using ExchangeRate.Domain.Entity.Currency;
using ExchangeRate.Domain.Entity.Currency.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Domain.Interface.Currency
{
    public interface ICurrencyRepository
    {
        Task<bool> CreateCurrency(CurrencyInfo currencyInfo);
        Task<List<CurrencyInfo>> SelectCurrency(CurrencyRequest request);
    }
}
