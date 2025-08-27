using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Interface.Authenticate
{
    public interface IAuthenticateService
    {
        Task<bool> AutheticateAsync(string email, string password);
        public string GenerateToken(string name, string email, string role);

    }
}
