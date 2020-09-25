using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_MenuType")]
    public class MenuType : BaseModel
    {
        public long ID { get; set; }

        public string MenuTypeName { get; set; }

    }
}
