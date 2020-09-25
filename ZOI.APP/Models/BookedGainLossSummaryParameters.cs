using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZOI.BAL.Models;

namespace ZOI.APP.Models
{
    public class BookedGainLossSummaryParameters
    {
        public int Amc { get; set; }
        public int Scheme { get; set; }

        public int Customer { get; set; }

        public int PortfolioType { get; set; }

        public int Account { get; set; }

        public int CommonAccount { get; set; }

        public string CommonDate { get; set; }

        public string Date { get; set; }

        public string UserID { get; set; }
        public int Flag { get; set; }


    }
}
