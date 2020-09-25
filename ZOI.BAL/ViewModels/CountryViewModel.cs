using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class CountryViewModel : BaseViewModel
    {
        public CountryViewModel()
        {
            this.country = new Country();
        }


        public Country country { get; set; }
        public IEnumerable<SelectListItem> TimeZoneList { get; set; }
    }
}
