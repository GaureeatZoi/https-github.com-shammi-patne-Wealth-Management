using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZOI.BAL.Models;

namespace ZOI.APP.Models
{
    public class DashboardParameters : AjaxDataTable
    {
        public string AsOnDate { get; set; }
        public string InceptionDate { get; set; }
        public int CustomerLevel { get; set; }

        public int ProtoTypeLevel { get; set; }

        public int AccountLevel { get; set; }

        public string ReportDate { get; set; }
        public int isProductOrAsset { get; set; }
        public string UserID { get; set; }
    }
}
