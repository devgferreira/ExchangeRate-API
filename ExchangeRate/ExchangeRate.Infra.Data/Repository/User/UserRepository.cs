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
                        (name, email, passwordHash, passwordSalt, createdAt)
                        VALUES(@Name, UPPER(@Email), @PasswordHash, @PasswordSalt, @CreatedAt)";
            await _dbContext.Connection.ExecuteAsync(sql, new
            {
                userInfo.Name,
                userInfo.Email,
                userInfo.PasswordHash,
                userInfo.PasswordSalt,
                userInfo.CreatedAt
            });
     
        }

        public async Task<List<UserInfo>> SelectUser(UserRequest request)
        {
            var sql = "SELECT id, name, email, passwordHash, passwordSalt, createdAt FROM Users WHERE 1 = 1 ";
            if (!string.IsNullOrEmpty(request.Email))
            {
                sql += " AND UPPER(email) = UPPER(@Email)";
            }
            if (request.Id != null)
            {
                sql += " AND id = @Id";
            }
            var result = await _dbContext.Connection.QueryAsync<UserInfo>(sql, new
            {
                request.Email,
                request.Id
            });
            return result.ToList();
        }
    }
}
