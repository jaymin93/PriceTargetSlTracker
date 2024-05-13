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
        public decimal Tgt1 { get; set; }
        public decimal Tgt2 { get; set; }
        public decimal Tgt3 { get; set; }
        public decimal Tgt4 { get; set; }
        public decimal Tgt5 { get; set; }
        public decimal Tgt6 { get; set; }
        public bool isActive { get; set; }
    }
}
