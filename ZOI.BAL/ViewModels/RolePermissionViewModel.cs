using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class RolePermissionViewModel : BaseViewModel
    {
        public RolePermissionViewModel()
        {
            this.rolePermission = new MenuPermission();
        }

        public MenuPermission rolePermission  { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }

    }
}
