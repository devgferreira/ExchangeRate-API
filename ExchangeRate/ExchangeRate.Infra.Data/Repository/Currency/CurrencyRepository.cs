using ExchangeRate.Domain.Entity.Currency;
using ExchangeRate.Domain.Interface;
using ExchangeRate.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infra.Data.Repository.Currency
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly DbContext _dbContext;

        public CurrencyRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateCurrency(CurrencyInfo currencyInfo)
        {
            throw new NotImplementedException();
        }
    }
}
