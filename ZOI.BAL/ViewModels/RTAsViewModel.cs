using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class RTAsViewModel : BaseViewModel
    {
        public RTAsViewModel()
        {
            this.rTAs = new RTAs();
        }
        public RTAs rTAs { get; set; }

    }

}

