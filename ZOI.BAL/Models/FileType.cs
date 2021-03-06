﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_FileTypes")]
    public class FileType: BaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
