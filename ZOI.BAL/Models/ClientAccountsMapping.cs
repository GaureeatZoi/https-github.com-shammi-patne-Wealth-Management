using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_L5_ClientAccounts")]
    public class ClientAccountsMapping : BaseModel
    {

        [Required(ErrorMessage = "Please select the Client")]
        [Display(Name = "Client")]
        [Column("L3_ClientID")]
        public long ClientID {get; set; }
        [NotMapped] public string Client { get; set; }
        [NotMapped] public string PrimaryRM { get; set; }
        [NotMapped] public string SecondaryRM { get; set; }
        [NotMapped] public string Accounts { get; set; }


        public long ID { get; set; }

        [Required(ErrorMessage = "Please select the Account Type")]
        [Display(Name = "Account Type")]
        [Column("L4_AccountTypeID")]
        public long AccountTypeID { get; set; }


        [Required(ErrorMessage = "Please enter the Broker UCC")]
        [Display(Name = "UCC")]
        [RegularExpression("^([A-Z]){5}([0-9]){5}$", ErrorMessage = "Invalid Broker UCC")]
        public string UCC { get; set; }

        [Display(Name = "Is Default")]
        public bool IsDefault { get; set; }

        [Display(Name = "Effective From")]
        [Required(ErrorMessage = "Please enter the Effective From")]
        public string  EffectiveFrom{ get; set; }

        [Display(Name = "Effective To")]
        public string EffectiveTo { get; set; }

        [Required(ErrorMessage = "Please select the RM")]
        [Display(Name = "Primary RM")]
        public long RMId { get; set; }

        [Required(ErrorMessage = "Please select the Secondary RM")]
        [Display(Name = "Secondary RM")]
        public long SecondaryRMId { get; set; }


    }
}
