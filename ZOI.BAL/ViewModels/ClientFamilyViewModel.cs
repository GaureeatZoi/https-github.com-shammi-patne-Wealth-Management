using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class ClientFamilyViewModel : BaseViewModel
    {
        public ClientFamilyViewModel()
        {
            this.clientFamily = new ClientFamily();
        }

        public ClientFamily clientFamily { get; set; }

       
        public IEnumerable<SelectListItem> GroupList { get; set; }

        
        public IEnumerable<SelectListItem> CountryList { get; set; }

       
        public IEnumerable<SelectListItem> StateList { get; set; }

       
        public IEnumerable<SelectListItem> CityList { get; set; }

    }

}

