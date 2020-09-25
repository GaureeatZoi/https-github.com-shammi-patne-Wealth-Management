using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class RoleViewModel : BaseViewModel
    {
        public RoleViewModel()
        {
            this.role = new Role(); 
        }

     public Role role { get; set; } 
     public IEnumerable<SelectListItem> RoleList { get; set; }
     public IEnumerable<SelectListItem> ApplicationList { get; set; }

    }
}
