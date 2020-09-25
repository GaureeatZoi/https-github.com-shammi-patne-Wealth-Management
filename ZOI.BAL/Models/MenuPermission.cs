using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_RoleMenuMapping")]
    public class MenuPermission : BaseModel
    {
        public long ID { get; set; }
        
        public int RoleID { get; set; }
        
        public long MenuID { get; set; }
        [ForeignKey("MenuID")]
        public virtual Menu Menus { get; set; }

        [Column("ReadAccess")]
        public bool Read { get; set; }

        [Column("WriteAccess")]
        public bool Write { get; set; }
        
        public bool ExportExcel { get; set; }
        
        public bool ExportPDF { get; set; }
        
        public bool ExportCSV { get; set; }

        [Column("SettingsAccess")]
        public bool Settings { get; set; }
              

    }
}
