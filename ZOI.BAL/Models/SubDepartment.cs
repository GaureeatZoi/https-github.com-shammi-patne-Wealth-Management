using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ZOI.BAL.Models
{
    [Table("Tbl_SubDepartment")]
   public  class SubDepartment : Base.BaseModel
    {
        public int ID { get; set; }
        public int DepartmentId { get; set; }
        public string SubDepartmentName { get; set; }

    }
}
