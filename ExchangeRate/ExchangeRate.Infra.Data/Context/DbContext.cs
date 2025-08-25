using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infra.Data.Context
{
    public class DbContext : IDisposable
    {
        public IDbConnection Connection { get; set; }
        public DbContext(IConfiguration configuration)
        {
            Env.Load();
            var connStr = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                  ?? configuration.GetConnectionString("DefaultConnection");

            Connection = new NpgsqlConnection(connStr);
            Connection.Open();
        }
        public void Dispose() => Connection?.Dispose();
    }
}
