using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace ZOI.BAL.Models
{
    public class ProductReportFilters
    {
        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [Display(Name = "AMC / Exchange")]
        public string AMC { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        public IEnumerable<SelectListItem> ProductNameList { get; set; }
        public IEnumerable<SelectListItem> AmcExchangeList { get; set; }
        public IEnumerable<SelectListItem> TransactionTypeList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }

    }
}
