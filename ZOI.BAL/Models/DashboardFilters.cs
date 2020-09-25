using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZOI.BAL.Models
{
    public class DashboardFilters
    {
        [Display(Name = "Select Customer")]
        public string CustomerLevel { get; set; }

        [Display(Name = "Portfolio Type")]
        public string ProtoTypeLevel { get; set; }

        [Display(Name = "Select Account")]
        public string AccountLevel { get; set; }

        [Display(Name = "Inception Date")]
        public string ReportDate { get; set; }

        public IEnumerable<SelectListItem> CustomerLevelList { get; set; } 
        public IEnumerable<SelectListItem> ProtoTypeLevelList { get; set; }
        public IEnumerable<SelectListItem> AccountLevelList { get; set; }
    }
}
