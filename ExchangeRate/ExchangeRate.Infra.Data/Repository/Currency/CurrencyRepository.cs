using Dapper;
using ExchangeRate.Domain.Entity.Currency;
using ExchangeRate.Domain.Entity.Currency.Request;
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
                        (code, codein, bid, ask, DateOfCurrency, CreatedAT)
                        VALUES(@Code, @Codein, @Bid, @Ask, @DateOfCurrency, @CreatedAT)
                        ON CONFLICT (DateOfCurrency) DO NOTHING;";

            var rowsAffected = await _dbContext.Connection.ExecuteAsync(sql, new
            {
                currencyInfo.Code,
                currencyInfo.Codein,
                currencyInfo.Bid,
                currencyInfo.Ask,
                currencyInfo.DateOfCurrency,
                currencyInfo.CreatedAT
            });

            return rowsAffected > 0;
        }

        public async Task<List<CurrencyInfo>> SelectCurrency(CurrencyRequest request)
        {
            var sql = "SELECT code, codein, bid, ask, dateofcurrency, createdat FROM Currency WHERE 1 = 1 ";

            if(!string.IsNullOrEmpty(request.Code))
            {
                sql += " AND code = @Code";
            }
            if (!string.IsNullOrEmpty(request.CodeIn))
            {
                sql += " AND codein = @CodeIn";
            }
            if (request.DateOfCurrency != null)
            {
                sql += " AND dateofcurrency::date = @DateOfCurrency";
            }

            var result = await _dbContext.Connection.QueryAsync<CurrencyInfo>(sql, new
            {
                request.Code,
                request.CodeIn,
                request.DateOfCurrency
            });
            return result.ToList();


        }
    }
}
