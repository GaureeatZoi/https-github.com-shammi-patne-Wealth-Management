using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("Tbl_MenuMaster")]
    public class Menu : BaseModel
    {
        public long ID { get; set; }

        
        [DisplayName("Menu Name")]
        [Required(ErrorMessage = "Please enter the Menu Name")]
        public string MenuName { get; set; }

        
        [DisplayName("Is Parent Menu")]
        public bool IsParentMenu { get; set; } 
        
        [DisplayName("Is Sub-Parent Menu")]
        [Column("IsSubParentMenu")]
        public bool IsSubMenu { get; set; }

        [DisplayName("Parent Menu")]
        public long?  ParentMenuId { get; set; }
        
        [DisplayName("Group")]
        public long?  GroupID { get; set; }

        [DisplayName("Sub Parent Menu")]
        public long? SubParentMenuID { get; set; }

        [DisplayName("Controller")]
        public string ControllerName { get; set; }

        [DisplayName("Action")]       
        public string ActionName { get; set; }

        [DisplayName("Parameter 1")]
        public string Params1 { get; set; }

        [DisplayName("Parameter 2")]
        public string Params2 { get; set; }

        [DisplayName("Menu Icon")]
        [Required(ErrorMessage = "Please enter the Menu Icon")] 
        public string MenuIcon { get; set; }        

        [DisplayName("Menu Order")]
        [Required(ErrorMessage = "Please enter the Menu Order")] 
        public int? MenuOrder  { get; set; }

        [NotMapped]
        public string IsParentMenuText { get; set; }
        [NotMapped]
        public string ParentMenuName { get; set; }
 
        
    }
}
