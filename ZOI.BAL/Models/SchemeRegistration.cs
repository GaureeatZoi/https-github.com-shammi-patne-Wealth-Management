using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Scheme_Registrationdetails")]
    public class SchemeRegistration : BaseModel
    {
        [Key]
        public long ID { get; set; }
        public long SchemeID { get; set; }
        //Registration
        [Display(Name = "AMFI Code")]
        [RegularExpression("[A-Za-z0-9 ]*", ErrorMessage = "Invalid AMFI Code")]
        [Required(ErrorMessage = "Please enter AMFI Code")]
        public string AMFICode { get; set; }

        [Display(Name = "Option Type")]
        [Required(ErrorMessage = "Please select Option Type ")]
        public int SchemeOption { get; set; }

        [Display(Name = "Series")]
        [Required(ErrorMessage = "Please select Series")]
        [Column(name: "Series")]
        public int SeriesId { get; set; }

        [Display(Name = "NSE Symbol")]
        [Required(ErrorMessage = "Please enter NSE Code ")]
        public string NSESymbol { get; set; }

        [Display(Name = "BSE Code")]
        [Required(ErrorMessage = "Please enter BSE Code  ")]
        public string BseCode { get; set; }

        [Display(Name = "ISIN")]
        [Required(ErrorMessage = "Please enter ISIN  ")]
        public string ISIN { get; set; }

        [Display(Name = "Ref ISIN")]
        [Column(name: "OldISIN")]
        public string RefISIN { get; set; }

        [Display(Name = "Maturity Date")]
        [Required(ErrorMessage = "Please enter Maturity Date")]
        public DateTime? MaturityDate { get; set; }

        [Display(Name = "Issue Open Date")]
        [Required(ErrorMessage = "Please enter Issue Open Date  ")]
        public DateTime? IssueOpenDate { get; set; }

        [Display(Name = "Issue Close Date")]
        [Required(ErrorMessage = "Please enter Issue Close Date  ")]
        [Column(name: "IssueCloseDate")]
        public DateTime? CloseDate { get; set; }

        [Display(Name = "ReOpen Date")]
        [Required(ErrorMessage = "Please enter Issue Reopen Date")]
        [Column(name: "IssueReopenDate")]
        public DateTime? ReOpenDate { get; set; }
    }
}
