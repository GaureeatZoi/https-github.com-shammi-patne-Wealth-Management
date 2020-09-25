using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZOI.BAL.Models
{
    public class MFSummaryFilter
    {
        [Display(Name = "Select Asset Class")]
        public string AssetClass { get; set; }

        [Display(Name = "Select AMC")]
        public string AMC { get; set; }

        [Display(Name = "Select Type")]
        public string Type { get; set; }

        [Display(Name = "XIRR Range")]
        public string XIRRRange { get; set; }

        public IEnumerable<SelectListItem> AssetClassList { get; set; }
        public IEnumerable<SelectListItem> AMCList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
    }
}
