using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockLiveDashboard.Contracts.DTOs
{
    public class StockInfoByInvestor
    {
        public int Buyers { get; set; }
        public int Sellers { get; set; }
        public int Holding { get; set; }
    }
}
