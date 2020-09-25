using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZOI.BAL.Models
{
    public class BookedGrainLossFilters
    {

        [Display(Name = "Select Account")]
        public string CustomerLevel { get; set; }

        [Display(Name = "AMC")]
        public string AMC { get; set; }

        [Display(Name = "Scheme")]
        public string Scheme { get; set; }

        [Display(Name = "Date")]
        public string ReportDate { get; set; }
        public IEnumerable<SelectListItem> CustomerLevelList { get; set; }
        public IEnumerable<SelectListItem> AMCLevelList { get; set; }
        public IEnumerable<SelectListItem> SchemeList { get; set; }
    }
}
