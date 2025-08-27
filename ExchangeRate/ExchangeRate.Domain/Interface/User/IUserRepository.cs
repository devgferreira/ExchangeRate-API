using ExchangeRate.Domain.Entity.User;
using ExchangeRate.Domain.Entity.User.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Domain.Interface.User
{
    public interface IUserRepository
    {
        Task CreateUser(UserInfo userInfo);
    }
}
