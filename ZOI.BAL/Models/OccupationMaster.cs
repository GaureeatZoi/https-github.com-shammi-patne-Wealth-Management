using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_occupations")]
    public class OccupationMaster:BaseModel
    {
        public long ID { get; set; }


        [Required(ErrorMessage = "Please enter Occupation")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid Occupation")]
        public string Occupation { get; set; }
    }
}
