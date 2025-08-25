using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Settings
{
    public interface IApplicationSettings
    {
        string URLAwesomeAPI { get; }
        string ConnectionString { get; }
    }
}
