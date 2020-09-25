using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_HoldingNature")]
    public class HoldingNature:BaseModel
    {
        public long  ID { get; set; }

        [Required(ErrorMessage = "Please enter HNCode")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid HNCode")]
        public string HNCode { get; set; }

        [Required(ErrorMessage = "Please enter HoldingType")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid HoldingType")]
        public string HoldingType { get; set; }
    }
}
