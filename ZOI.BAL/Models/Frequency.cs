using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_SchemeFrequency")]
    public class Frequency:BaseModel
    {
        public long ID { get; set; }

        public long SchemeID { get; set; }
        public bool Daily { get; set; }
        public bool Weekly { get; set; }
        public bool Monthly { get; set; }
        public bool Quartely { get; set; }

        [Display(Name = "Half-Yearly")]
        public bool HalfYearly { get; set; }
        public bool Yearly { get; set; }
    }
}
