using Dapper;
using ExchangeRate.Domain.Entity.User;
using ExchangeRate.Domain.Entity.User.Request;
using ExchangeRate.Domain.Interface.User;
using ExchangeRate.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infra.Data.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _dbContext;

        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateUser(UserInfo userInfo)
        {
            var sql = @"INSERT INTO Users
                        (name, email, passwordHash, passwordSalt, role, createdAt)
                        VALUES(@Name, UPPER(@Email), @PasswordHash, @PasswordSalt, @Role, @CreatedAt)";
            await _dbContext.Connection.ExecuteAsync(sql, new
            {
                userInfo.Name,
                userInfo.Email,
                userInfo.PasswordHash,
                userInfo.PasswordSalt,
                userInfo.CreatedAt
            });
     
        }

        public Task<List<UserInfo>> SelectUser(UserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
