using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ZOI.BAL.Models
{
    [Table("tbl_FranchiseMapping")]
    public class FranchiseMapping : Base.BaseModel
    {
        public long ID { get; set; }
        public long EntityID { get; set; }
    }
}
