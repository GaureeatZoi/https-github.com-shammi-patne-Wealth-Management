using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Accounttypes")]
    public class AccountTypes : BaseModel
    {
        public long ID { get; set; }
        
        public long AccountTypeCode { get; set; }
        
        public long AccountTypeName { get; set; }

    }
}
