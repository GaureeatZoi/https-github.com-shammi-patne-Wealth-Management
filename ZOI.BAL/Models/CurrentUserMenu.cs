using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    public class CurrentUserMenu : MenuPermissionList 
    {
        public long ID { get; set; }

        public string MenuName { get; set; }

        public bool IsParentMenu { get; set; }
        
        public long? ParentMenuId { get; set; }
       
        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string Params1 { get; set; }

        public string Params2 { get; set; }

        public string MenuIcon { get; set; }

        public int MenuOrder { get; set; }

        public long SubParentMenuID { get; set; }
        
        public long GroupID { get; set; }
        
        public bool IsSubMenu { get; set; }

    }
}
