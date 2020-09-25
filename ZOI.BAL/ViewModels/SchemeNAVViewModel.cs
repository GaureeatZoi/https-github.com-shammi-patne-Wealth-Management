using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class SchemeNAVViewModel : BaseViewModel
    {
        public SchemeNAVViewModel()
        {
            this.schemenav = new SchemeNAV();
        }



        public SchemeNAV schemenav { get; set; }
    }

   
}
