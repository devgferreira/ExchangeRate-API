using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.DTO.AwesomeAPI
{
    public class AwesomeAPIDTO
    {
        public string Code { get; set; }
        public string Codein { get; set; }
        public string Name { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string VarBid { get; set; }
        public string PctChange { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public string Timestamp { get; set; }
        public string Create_date { get; set; }
    }
}
