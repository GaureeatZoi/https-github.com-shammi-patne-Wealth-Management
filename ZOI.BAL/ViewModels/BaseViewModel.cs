using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class BaseViewModel
    {
        public IEnumerable<CurrentUserMenu> Menus { get; set; }

        public IEnumerable<MenuPermission> MenuPermissions { get; set; }

        public MenuPermission CurrentMenuPermission { get; set; }

        public IEnumerable<Claim> UserClaims { get; set; }

        public bool IsEdit { get; set; }

    }
}
