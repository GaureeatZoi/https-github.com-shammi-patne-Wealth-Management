using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ZOI.BAL.Models
{
    [Table("Tbl_SchemeNAV")]
    public class SchemeNAV : Base.BaseModel
    {
        public long ID { get; set; }

        public string RTAName { get; set; }

        public string AMCName { get; set; }

        public string SchemeName { get; set; }

        public decimal NAV { get; set; }

        public DateTime NAVDate { get; set; }



    }
   
}
