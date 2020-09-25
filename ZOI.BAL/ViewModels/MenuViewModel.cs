using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ZOI.BAL.Models;


namespace ZOI.BAL.ViewModels
{
    public class MenuViewModel :BaseViewModel
    {
        public MenuViewModel()
        {
            this.menu = new Menu();
        }
    
    public Menu menu { get; set; }

    public IEnumerable<SelectListItem> MenuList { get; set; }
    public IEnumerable<SelectListItem> SubMenuList { get; set; }
    public IEnumerable<SelectListItem> GroupList { get; set; }
    
    }
}
