using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Scheme_TransactionalDetails")]
    public class SchemeTransaction : BaseModel
    {
        [Key]
        public long ID { get; set; }
        public long SchemeID { get; set; }

        //Transaction
        [Display(Name = "One Time/Recurring")]
        public bool IsRecurring { get; set; }

        [Display(Name = "SIP Allowed")]
        [Column(name:"IsSIPAllowed")]
        public bool SIP { get; set; }

        [Display(Name = "STP Allowed")]
        [Column(name: "IsSTPAllowed")]
        public bool STP { get; set; }


        [Display(Name = "SWP Allowed")]
        [Column(name: "IsSWPAllowed")]
        public bool SWP { get; set; }

        [Display(Name = "Demat Allowed")]
        [Column(name: "IsDematAllowed")]
        public bool Demat { get; set; }

        [Display(Name = "Included Units Of ExDate")]

        [Column(name: "IncludedUnitsExDate")]
        public bool IncludedUnitsOfExDate { get; set; }

        [Display(Name = "SIP Date")]
        public DateTime? SIPDate { get; set; }

        [Display(Name = "STP Date")]
        public DateTime? STPDate { get; set; }

        [NotMapped]
        [Display(Name = "Restricted Nationality")]
        [Required(ErrorMessage = "Please select Restricted Nationality ")]
        public string[] RestrictedNationality { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please select InvestorType")]
        [Display(Name = "Investor Type")]
        public string[] InvestorType { get; set; }

        [Display(Name = "No Of Days")]
        [Required(ErrorMessage = "Please select Minimum Holding Period Days  ")]
        [Column(name: "MinimumHoldingPeriod_Days")]
        public int NoOfDays { get; set; }

        [Display(Name = "No Of Months")]
        [Required(ErrorMessage = "Please select Minimum Holding Period Months  ")]
        [Column(name: "MinimumHoldingPeriod_Months")]
        public int NoOfMonths { get; set; }

        [Display(Name = "No Of Years")]
        [Required(ErrorMessage = "Please select Minimum Holding Period Months  ")]
        [Column(name: "MinimumHoldingPeriod_Years")]
        public int NoOfYears { get; set; }

        public string Ratio { get; set; }
    }
}
