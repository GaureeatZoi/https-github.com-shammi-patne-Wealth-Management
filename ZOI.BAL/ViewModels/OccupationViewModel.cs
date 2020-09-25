using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class OccupationViewModel:BaseViewModel
    {
        public OccupationViewModel()
        {
            OccupationMaster = new OccupationMaster();
        }
        public OccupationMaster OccupationMaster { get; set; }
    }
}
