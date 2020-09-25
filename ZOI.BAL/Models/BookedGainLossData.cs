using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZOI.BAL.Models
{
    public class BookedGainLossData
    {
        public int ID { get; set; }

        public string Scheme { get; set; }

        public int Quantity { get; set; }

        public string PurchaseDates { get; set; }

        public int PurchaseValue { get; set; }

        public string SaleDates { get; set; }

        public int SaleValue { get; set; }

        public int STT { get; set; }

        public int Gain { get; set; }

        public string DaysInvesteds { get; set; }

        public decimal ShortTermAmount { get; set; }

        public decimal LongTermAmount { get; set; }

        public string LastUpdateOn { get; set; }

    }
}
