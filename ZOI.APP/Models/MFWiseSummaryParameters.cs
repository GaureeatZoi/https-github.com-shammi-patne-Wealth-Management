using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZOI.BAL.Models;

namespace ZOI.APP.Models
{
    public class MFWiseSummaryParameters : AjaxDataTable
    {
        public string InceptionDate { get; set; }
        public int CustomerLevel { get; set; }

        public int ProtoTypeLevel { get; set; }

        public int AccountLevel { get; set; }

        public int AssetClass { get; set; }

        public int AMC { get; set; }

        public int Type { get; set; }

        public string FromXIRR { get; set; }
        public string ToXIRR { get; set; }
        public string UserID { get; set; }
        public int SchemeID { get; set; }
    }
}
