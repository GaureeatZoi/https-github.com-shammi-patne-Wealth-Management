using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_WealthManagementCompanies")]
    public class CompanyMaster:BaseModel
    {
        public long ID { get; set; }

        [Display(Name = "Icon Name")]
        [Required(ErrorMessage = "Please enter the Icon Name")]
        public string IconName { get; set; }
        [NotMapped]
        public string Icon { get; set; }

        [Column("Icon")]
        public byte[] IconFileData { get; set; }

        [NotMapped]
        public IFormFile IconFile { get; set; }

        [Display(Name = "Name Of Company")]
        [Required(ErrorMessage = "Please enter Name Of Company")]
        [RegularExpression("[A-Za-z0-9 ]*", ErrorMessage = "Invalid Company Name")]
        public string NameOfCompany { get; set; }

        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Please enter the Address Line 1")]
        public string AddressLine1 { get; set; }


        [Display(Name = "Address Line 2")]
        [Required(ErrorMessage = "Please enter the Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Address Line 3")]
        [Required(ErrorMessage = "Please enter the Address Line 2")]
        public string AddressLine3 { get; set; }

        [Display(Name = "Select Country")]
        [Required(ErrorMessage = "Please select Country")]
        public long CountryID { get; set; }
        [NotMapped]
        public string CountryName { get; set; }


        [Display(Name = "Select State")]
        [Required(ErrorMessage = "Please select State")]
        public long StateID { get; set; }
        [NotMapped]
        public string StateName { get; set; }


        [Display(Name = "Select City")]
        [Required(ErrorMessage = "Please select City")]
        public long CityID { get; set; }
        [NotMapped]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Please enter Pincode")]
        [RegularExpression("^([0-9]){6}$", ErrorMessage = "Invalid Pincode")]
        public long? PinCode { get; set; }

        [Display(Name = "Contact Person Name")]
        [Required(ErrorMessage = "Please enter Contact Person Name")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid Contact Person Name")]
        public string ContactPersonName { get; set; }

        [Display(Name = "Contact Person Email")]
        [Required(ErrorMessage = "Please enter Contact Person Email")]
        public string ContactPersonEmail { get; set; }

        [Display(Name = "Contact Person Mobile")]
        [Required(ErrorMessage = "Please Contact Person Mobile")]
        [RegularExpression("^([0-9]){10}$", ErrorMessage = "Invalid Mobile No.")]
        public string ContactPersonMobile { get; set; }

        [Display(Name = "Company Code")]
        public string CompanyCode { get; set; }
    }
}
