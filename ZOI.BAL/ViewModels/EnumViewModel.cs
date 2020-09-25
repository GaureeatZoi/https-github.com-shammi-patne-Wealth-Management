using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class EnumViewModel : BaseViewModel
    {
        public EnumViewModel()
        {
            this.EnumMaster = new ZOI.BAL.Models.Enum();
        }

       public ZOI.BAL.Models.Enum EnumMaster { get; set; }
    }
}
