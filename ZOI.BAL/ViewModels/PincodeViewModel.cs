using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class PincodeViewModel : BaseViewModel
    {
        public PincodeViewModel()
        {
            this.pincode = new PincodeMaster();
        }
        
        
        public PincodeMaster pincode { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}
