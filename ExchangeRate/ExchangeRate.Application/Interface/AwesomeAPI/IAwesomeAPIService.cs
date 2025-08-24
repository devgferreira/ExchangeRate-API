using ExchangeRate.Application.DTO.AwesomeAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Interface.AwesomeAPI
{
    public interface IAwesomeAPIService
    {
        Task<AwesomeAPIDTO> GetLastCurrencyAsync(string currency);
    }
}
