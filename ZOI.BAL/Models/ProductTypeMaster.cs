using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;
using ZOI.BAL.Utilites;

namespace ZOI.BAL.Models
{
    [Table("tbl_ProductTypes")]
    public class ProductTypeMaster : BaseModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = Constants.Message.ProductTypeMaster.Code_Required)]
        [Column("ProductCode")]
        [Display(Name = "Product Code")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = Constants.Message.ProductTypeMaster.Code_Expression)]
        [MaxLength(5, ErrorMessage = Constants.Message.ProductTypeMaster.Code_MaxLength), MinLength(2, ErrorMessage = Constants.Message.ProductTypeMaster.Code_MinLength)]
        public string Code { get; set; }

        [Required(ErrorMessage = Constants.Message.ProductTypeMaster.Name_Required)]
        [Column("ProductName")]
        [Display(Name = "Name")]
        [MaxLength(500, ErrorMessage = Constants.Message.ProductTypeMaster.Name_MaxLength), MinLength(5, ErrorMessage = Constants.Message.InvestorCategorys.Name_MinLength)]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = Constants.Message.ProductTypeMaster.Name_Expression)]
        public string Name { get; set; }
         
        [Required(ErrorMessage = Constants.Message.ProductTypeMaster.Description_Required)]
        [Column("ProductDescription")]
        [Display(Name = "Description")]
        [MaxLength(500, ErrorMessage = Constants.Message.ProductTypeMaster.Description_MaxLength), MinLength(5, ErrorMessage = Constants.Message.ProductTypeMaster.Description_MinLength)]
        public string ProductDescription { get; set; }
         
        [Required(ErrorMessage = Constants.Message.ProductTypeMaster.KYCDescription_Required)]
        [Column("ProductKYCDescription")]
        [Display(Name = "KYC Description")]
        [MaxLength(500, ErrorMessage = Constants.Message.ProductTypeMaster.KYCDescription_MaxLength), MinLength(5, ErrorMessage = Constants.Message.ProductTypeMaster.KYCDescription_MinLength)]
        public string ProductKYCDescription { get; set; }




    }
}
