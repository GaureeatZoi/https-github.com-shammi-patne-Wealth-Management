using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class StateViewModel : BaseViewModel
    {
        public StateViewModel()
        {
            this.state = new State();
        }
        public State state { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
    }
}
