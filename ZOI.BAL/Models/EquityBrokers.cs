using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_RegisteredBrokers")]
    public class EquityBrokers : BaseModel
    {
        public long ID { get; set; }

        [Display(Name = "Equity Broker Name")]
        [Required(ErrorMessage = "Please enter the Name")]
        [Column("BrokerName")]
        public string Name { get; set; }

        [Column("BrokerLogo")]
        public string Logo { get; set; }

        [NotMapped]
        public byte[] LogoFileData { get; set; }

        [NotMapped]
        public IFormFile LogoFile { get; set; }


        [Required(ErrorMessage = "Please enter the NSE member code")]
        [Display(Name = "NSE Member Code")]
        public string NSEMembercode { get; set; }

        [Display(Name ="BSE Member Code")]
        [Required(ErrorMessage = "Please enter the BSE member code")]
        public string BSEMembercode { get; set; }

        [Display(Name = "MCX Member Code")]
        [Required(ErrorMessage = "Please enter the MCX member code")]
        public string MCXMembercode { get; set; }


        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Please enter the Address Line 1")]
        public string AddressLine1 { get; set; }



        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }


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


        [Display(Name = "Pincode")]
        [Required(ErrorMessage = "Please enter the Pincode")]
        public long? PinCode { get; set; }

        [Display(Name = "Select Company")]
        [Required(ErrorMessage = "Please select Company")]
        public long CompanyID { get; set; }


    }
}
