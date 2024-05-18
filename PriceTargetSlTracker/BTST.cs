using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceTargetSlTracker
{
    public class BTST
    {
        public string symbol { get; set; }
        public decimal cmp { get; set; }
        public decimal buyprice { get; set; }
        public decimal SL { get; set; }
        public List<decimal> TargetList { get; set; }
        public bool isActive { get; set; }
    }
}
