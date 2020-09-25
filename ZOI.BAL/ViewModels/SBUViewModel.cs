using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class SBUViewModel : BaseViewModel
    {
        public SBUViewModel()
        {
            this.sBU = new SBU();
        }

        public SBU sBU { get; set; }

    }

}

