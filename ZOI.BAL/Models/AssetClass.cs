using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_AssetClass")]
    public class AssetClass : BaseModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter the Asset Class Code")]
        [Column("AssetClassCode")]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please enter the Asset Class Name")]
        [Column("AssetClassName")]
        [Display(Name = "Name")]
        public string Name { get; set; }       

    }
}
