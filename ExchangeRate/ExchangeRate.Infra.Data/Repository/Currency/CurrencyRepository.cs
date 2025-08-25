using Dapper;
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

        public async Task<bool> CreateCurrency(CurrencyInfo currencyInfo)
        {
            var sql = @"INSERT INTO Currency
                        (symbol, bid, ask, DateOfCurrency, CreatedAT)
                        VALUES(@Symbol, @Bid, @Ask, @DateOfCurrency, @CreatedAT)
                        ON CONFLICT (DateOfCurrency) DO NOTHING;";

            var rowsAffected = await _dbContext.Connection.ExecuteAsync(sql, new
            {
                currencyInfo.Symbol,
                currencyInfo.Bid,
                currencyInfo.Ask,
                currencyInfo.DateOfCurrency,
                currencyInfo.CreatedAT
            });

            return rowsAffected > 0;
        }
    }
}
