using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class ClientGroupViewModel : BaseViewModel
    {
        public ClientGroupViewModel()
        {
            this.clientGroup = new ClientGroup();
        }

        public ClientGroup clientGroup { get; set; }

       
        public IEnumerable<SelectListItem> CountryList { get; set; }

        
        public IEnumerable<SelectListItem> StateList { get; set; }

       
        public IEnumerable<SelectListItem> CityList { get; set; }

    }

}

