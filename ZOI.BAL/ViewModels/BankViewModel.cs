using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class BankViewModel : BaseViewModel
    {
        public BankViewModel()
        {
            this.bankMaster = new Bank();
        }

        public Bank bankMaster { get; set; }

       

    }

}

