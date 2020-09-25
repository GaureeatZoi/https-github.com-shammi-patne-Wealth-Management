using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class CityViewModel : BaseViewModel
    {
        public CityViewModel()
        {
            this.city = new City();
        }

        public City city { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> TierList { get; set; }

    }
}
