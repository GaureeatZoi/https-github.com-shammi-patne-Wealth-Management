using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class InvestorCategoryViewModel : BaseViewModel
    {
        public InvestorCategoryViewModel()
        {
            this.investorCategorys = new InvestorCategorys();
        }

        public InvestorCategorys investorCategorys { get; set; }

       

    }

}

