﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ZOI.BAL.Models
{
     [Table("Tbl_Designation")]
     public  class Designation : Base.BaseModel
    {
        public int ID { get; set; }
        public string DesignationName{ get; set; }
    }
}
