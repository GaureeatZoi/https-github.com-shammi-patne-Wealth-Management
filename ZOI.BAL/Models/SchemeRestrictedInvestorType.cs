using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Scheme_RestrictedInvestorType")]
    public class SchemeRestrictedInvestorType:BaseModel
    {
        public long ID { get; set; }

        public long SchemeID { get; set; }

        public int  TypeId { get; set; }
    }
}
