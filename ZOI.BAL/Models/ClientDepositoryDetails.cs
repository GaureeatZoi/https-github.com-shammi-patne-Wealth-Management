using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Client_DepositoryDetails")]
    public class ClientDepositoryDetails : BaseModel
    {
        public long ID { get; set; }
        
        public long ClientID { get; set; }

        [Required(ErrorMessage = "Please enter the Depository")]
        [Display(Name = "Depository")]
        public long DpId { get; set; }

        [Required(ErrorMessage = "Please enter the Account Number")]
        [Display(Name = "Account Number")]
        [RegularExpression("^([0-9]){13}$", ErrorMessage = "Invalid Account Number")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Please enter the Effective From")]
        [Display(Name = "Effective From")]
        public string EffectiveFrom { get; set; }

        [Required(ErrorMessage = "Please enter the Effective To")]
        [Display(Name = "Effective To")]
        public string EffectiveTo { get; set; }

    }
}
