using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class HoldingNatureViewModel:BaseViewModel
    {
        public HoldingNatureViewModel()
        {
            HoldingNature = new HoldingNature();
        }
        public HoldingNature HoldingNature { get; set; }
    }
}
