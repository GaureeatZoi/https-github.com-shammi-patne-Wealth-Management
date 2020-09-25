using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_BankBranchs")]
    public class BankBranch : BaseModel
    {
        public long Id { get; set; }

        [Display(Name = "Select Bank")]
        [Required(ErrorMessage = "Please select Bank")]       
        public long BankID { get; set; }
        [NotMapped]
        public string BankName { get; set; }


        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter the Branch Name")]
        [Column("BranchName")]
        public string  Name { get; set; }

        [Display(Name = "IFSC Code")]
        [Required(ErrorMessage = "Please enter the IFSC Code")]
        [RegularExpression("^([A-Z]){4}([0-9]){7}$", ErrorMessage = "Invalid IFSC Code")]
        [MaxLength(11,ErrorMessage = "IFSC Code should not be Exsits 11 characters."),MinLength(11, ErrorMessage = "IFSC Code should not be below 11 characters.")]
        public string IFSCCode { get; set; }

        [Display(Name = "MICR Code")]
        [Required(ErrorMessage = "Please enter the MICR Code")]
        [RegularExpression("^([0-9]){9}$", ErrorMessage = "Invalid MICR Code")]
        [MaxLength(9, ErrorMessage = "MICR Code should not be Exsits 9 characters."), MinLength(9, ErrorMessage = "MICR Code should not be below 9 characters.")]
        public string MICRCode { get; set; }
            
        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Please enter the Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please select Country")]
        public int CountryID { get; set; }
        [NotMapped]
        public string CountryName { get; set; }


        [Display(Name = "State")]
        [Required(ErrorMessage = "Please select State")]
        public int StateID { get; set; }
        [NotMapped]
        public string StateName { get; set; }


        [Display(Name = "City")]
        [Required(ErrorMessage = "Please select City")]
        public int CityID { get; set; }
        [NotMapped]
        public string CityName { get; set; }


        [Display(Name = "Pincode")]
        [Required(ErrorMessage = "Please enter the Pincode")]
        public long? PinCode { get; set; }

    }
}
