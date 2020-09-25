using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Client_Addresses")]
    public class ClientAddresses : BaseModel
    {

        public long Coressponding_ID { get; set; }
        
        public long Current_ID { get; set; }
        
        public long ClientID { get; set; }


        [StringLength(500, MinimumLength = 10, ErrorMessage = "Address Field must be at least 10 characters.")]
        [Required(ErrorMessage = "Please enter the Address Line 1")]
        [Display(Name = "Address Line 1")]
        public string Current_AddressLine1 { get; set; }

        
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Address Field must be at least 10 characters.")]
        [Display(Name = "Address Line 2")]
        public string Current_AddressLine2 { get; set; }

        
        [Required(ErrorMessage = "Please select the Country")]
        [Display(Name = "Country")]
        public long Current_CountryID { get; set; }

        
        [Required(ErrorMessage = "Please select the State")]
        [Display(Name = "State")]
        public long Current_StateID { get; set; }

        
        [Required(ErrorMessage = "Please select the City")]
        [Display(Name = "City")]
        public long Current_CityID { get; set; }

        
        [Required(ErrorMessage = "Please enter the Pincode")]
        [Display(Name = "Pincode")]
        [RegularExpression("^([0-9]){6}$", ErrorMessage = "Invalid Pincode")]
        public long? Current_PinCode { get; set; }

        
        [Required(ErrorMessage = "Please enter the Address Line 1")]
        [Display(Name = "Address Line 1")]
        public string Coressponding_AddressLine1 { get; set; }

        
        [Display(Name = "Address Line 2")]
        [StringLength(500, MinimumLength =10 , ErrorMessage = "Address Field must be at least 10 characters.")]
        public string Coressponding_AddressLine2 { get; set; }

        
        [Required(ErrorMessage = "Please select the Country")]        
        [Display(Name = "Country")]
        public long Coressponding_CountryID { get; set; }

        
        [Required(ErrorMessage = "Please select the State")]
        [Display(Name = "State")]
        public long Coressponding_StateID { get; set; }

        
        [Required(ErrorMessage = "Please select the City")]
        [Display(Name = "City")]
        public long Coressponding_CityID { get; set; }

        
        [Required(ErrorMessage = "Please enter the Pincode")]
        [Display(Name = "Pincode")]
        [RegularExpression("^([0-9]){6}$", ErrorMessage = "Invalid Pincode")]
        public long? Coressponding_PinCode { get; set; }

    }
}
