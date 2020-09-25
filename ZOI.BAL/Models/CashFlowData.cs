using System;
using System.Collections.Generic;
using System.Text;

namespace ZOI.BAL.Models
{
    public class CashFlowData
    {
        public string CashFlow { get; set; }
        public string AmountInCrore { get; set; }
        public string LastUpdatedOn { get; set; }

        public List<CashFlowData> cashFlowDatasList { get; set; }
    }
}
