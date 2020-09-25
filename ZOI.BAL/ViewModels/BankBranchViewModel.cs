using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class BankBranchViewModel : BaseViewModel
    {
        public BankBranchViewModel()
        {
            this.bankBranchMaster = new BankBranch();
        }

        public BankBranch bankBranchMaster { get; set; }

        public IEnumerable<SelectListItem> BankList { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }

    }

}

