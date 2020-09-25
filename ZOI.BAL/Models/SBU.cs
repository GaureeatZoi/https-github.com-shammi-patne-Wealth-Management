using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_SBUs")]
    public class SBU : BaseModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "Please enter the SBU Code")]
        [Column("SBUCode")]
        [Display(Name = "SBU Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please enter the SBU Name")]
        [Column("SBUName")]
        [Display(Name = "Name")]
        public string Name { get; set; }       

    }
}
