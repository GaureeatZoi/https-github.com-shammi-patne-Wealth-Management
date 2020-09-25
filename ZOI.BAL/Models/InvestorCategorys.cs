using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;
using ZOI.BAL.Utilites;

namespace ZOI.BAL.Models
{
    [Table("tbl_InvestorCategorys")]
    public class InvestorCategorys : BaseModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage = Constants.Message.InvestorCategorys.Code_Required)]
        [Column("Code")]
        [Display(Name = "Investor category Code")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = Constants.Message.InvestorCategorys.Code_Expression)]
        [MaxLength(10, ErrorMessage = Constants.Message.InvestorCategorys.Code_MaxLength), MinLength(5, ErrorMessage = Constants.Message.InvestorCategorys.Code_MinLength)]
        public string Code { get; set; }

        [Required(ErrorMessage = Constants.Message.InvestorCategorys.Name_Required)]
        [Column("InvestorCategory")]
        [Display(Name = "Name")]
        [MaxLength(500, ErrorMessage = Constants.Message.InvestorCategorys.Name_MaxLength), MinLength(5, ErrorMessage = Constants.Message.InvestorCategorys.Name_MinLength)]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = Constants.Message.InvestorCategorys.Name_Expression)]
        public string Name { get; set; }

    }
}
