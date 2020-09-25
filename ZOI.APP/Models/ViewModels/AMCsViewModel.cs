using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZOI.APP.Models.Base;
using ZOI.BAL.Models;

namespace ZOI.APP.Models.ViewModels
{
    public class AMCsViewModel : BaseViewModel
    {
        public AMCsViewModel()
        {
            this.AMCs = new AMCs();
        }

        public AMCs AMCs { get; set; }

    }
}
