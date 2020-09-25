using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Client_BankDetails")]
    public class ClientBankDetails : BaseModel
    {

        public long ID { get; set; }
        
        public long ClientID { get; set; }

        
        [Required(ErrorMessage = "Please select the Account Type")]
        [Display(Name = "Account Type")]
        public int AccountType { get; set; }

        
        [Required(ErrorMessage = "Please enter the IFSC Code")]
        [RegularExpression("^([A-Z]){4}([0-9]){7}$", ErrorMessage = "Invalid IFSC Code")]
        [Display(Name = "IFSC Code")]
        public string IFSCCode { get; set; }


        [Required(ErrorMessage = "Please select the MICR Code")]
        [Display(Name = "MICR Code")]
        public int BranchMICRCode { get; set; }


        [Required(ErrorMessage = "Please select the Account Number")]
        [Display(Name = "Account Number")]
        [RegularExpression("^([0-9]){13}$", ErrorMessage = "Invalid Account Number")]
        public string BankAccountNumber { get; set; }
         
    
    }
}
