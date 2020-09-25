using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class EquityBrokersViewModel : BaseViewModel
    {
        public EquityBrokersViewModel()
        {
            this.equityBrokers = new EquityBrokers();
        }

        public EquityBrokers equityBrokers { get; set; }

        
        public IEnumerable<SelectListItem> CountryList { get; set; }
       
        public IEnumerable<SelectListItem> StateList { get; set; }
        
        public IEnumerable<SelectListItem> CityList { get; set; }

        public IEnumerable<SelectListItem> CompanyList { get; set; }

    }

}

