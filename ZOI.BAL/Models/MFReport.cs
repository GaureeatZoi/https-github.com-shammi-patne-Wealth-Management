using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    public class MFReport
    {
        public string ID { get; set; }
        public string AMC { get; set; }

        public string Scheme { get; set; }

        public string MaturityDate { get; set; }

        public string Units { get; set; }

        public string PurchaseNav { get; set; }
        public string ValueAtcost { get; set; }
        public string CurrentNAV { get; set; }
        public string CurrentMarketValuation { get; set; }
        public string Dividend { get; set; }
        public string AbsoluteGainOrLoss { get; set; }
        public string AbsoluteGainORLossPercentage { get; set; }
        public string XIRR { get; set; }
        public string AvgDaysInvested { get; set; }

        public string LastUpdatedOn { get; set; }
        public List<MFReport> MFList { get; set; }
    }
}
