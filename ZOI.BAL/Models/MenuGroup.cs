using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_MenuGroup")]
    public class MenuGroup : BaseModel
    {
        public long ID { get; set; }

        public string GroupName { get; set; }

    }
}
