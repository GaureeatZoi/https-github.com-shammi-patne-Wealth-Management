using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class RTATransactionTypeViewModel : BaseViewModel
    {
        public RTATransactionTypeViewModel()
        {
            this.RTATransactionTypes = new RTATransactionTypes();
        }

        public RTATransactionTypes  RTATransactionTypes { get; set; }

        public IEnumerable<SelectListItem> TransactionTypeList { get; set; }
        
        public IEnumerable<SelectListItem> RTAList { get; set; }

    }
}
