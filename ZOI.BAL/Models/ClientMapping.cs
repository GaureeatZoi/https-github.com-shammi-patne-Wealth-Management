using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Client_MappingDetails")]
    public class ClientMapping : BaseModel
    {

        public long ID { get; set; }
        
        public long ClientID { get; set; }
       
        public long AccountTypeID { get; set; }

        [Required(ErrorMessage = "Please select the SBU")]
        [Display(Name = "SBU")]
        public long SBUID { get; set; }

        [Required(ErrorMessage = "Please select the RM")]
        [Display(Name = "RM")]
        public long RMId { get; set; }

        [Required(ErrorMessage = "Please select the Secondary RM")]
        [Display(Name = "Secondary RM")]
        public long SecondaryRMId { get; set; }

        [Required(ErrorMessage = "Please select the Branch")]
        [Display(Name = "Branch")]
        public long BranchId { get; set; }

        [Required(ErrorMessage = "Please select the Model")]
        [Display(Name = "Model")]
        public int ModelId { get; set; }

        [Display(Name = "Head Of Family")]
        public bool HeadOfFamily { get; set; }

        [Display(Name = "KYC Valid")]
        public bool KYCValid { get; set; }

        [Required(ErrorMessage = "Please select the KYC Form No")]
        [Display(Name = "KYC Form No")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "A-Za-z0-9 .,-/ Address Field Allowed these characters only")]
        public string KYCFormNo { get; set; }

        [Required(ErrorMessage = "Please select the Effective From")]
        [Display(Name = "Effective From")]
        public string EffectiveFrom { get; set; }

        [Required(ErrorMessage = "Please select the KYC Effective To")]
        [Display(Name = "Effective To")]       
        public string EffectiveTo { get; set; }
    
    }

}
