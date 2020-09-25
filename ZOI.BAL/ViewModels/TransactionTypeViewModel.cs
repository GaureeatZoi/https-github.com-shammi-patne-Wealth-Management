using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class TransactionTypeViewModel : BaseViewModel
    {

        public TransactionTypeViewModel()
        {
            this.transactionTypes = new TransactionTypes();

        }

        public TransactionTypes  transactionTypes { get; set; }

        public List<SelectListItem> TransactionNatureList { get; set; }

    }
}
