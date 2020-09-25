using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class AMCsViewModel : BaseViewModel
    {
        public AMCsViewModel()
        {
            this.aMCs = new AMCs();
        }

        public AMCs aMCs { get; set; }

        public IEnumerable<SelectListItem> RTAsList { get; set; }

    }
}
