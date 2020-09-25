using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Relationships")]
    public class Relationship:BaseModel
    {
        public int ID { get; set; }

        [Display(Name = "Relationship")]
        [Required(ErrorMessage = "Please enter Relationship")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid Relationship")]
        [Column(name: "Relationship")]
        public string RelationshipName { get; set; }
    }
}
