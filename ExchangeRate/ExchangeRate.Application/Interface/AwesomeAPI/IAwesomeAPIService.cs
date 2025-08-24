using ExchangeRate.Application.DTO.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Interface.AwesomeAPI
{
    public interface IAwesomeAPIService
    {
        Task<CurrencyDTO> GetCurrencyAsync(string coin);
    }
}
