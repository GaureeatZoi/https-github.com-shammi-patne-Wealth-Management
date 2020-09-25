using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_ClientFolios")]
    public class ClientFolio : BaseModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter the Folio Number")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Folio Number should not be Exists 10 characters.")]
        [Display(Name = "Folio Number")]
        [RegularExpression("^[0-9A-Z/-]*$", ErrorMessage = "Invalid Folio Number")]
        public string FolioNo { get; set; }

        [Required(ErrorMessage = "Please select the Folio start date")]
        [Display(Name = "Folio Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d M yy}", ApplyFormatInEditMode = true)]
        public string FolioDate { get; set; }


        [StringLength(75, MinimumLength = 10, ErrorMessage = "Investor Name must between 03 to 75 characters.")]
        [Required(ErrorMessage = "Please enter the Investor Name")]
        [Display(Name = "Investor Name")]
        [RegularExpression("^[a-z A-Z]*$", ErrorMessage = "Investor Name allow Alphabets with space only")]
        public string InvestorName { get; set; }

        [Required(ErrorMessage = "Please select the RTA")]
        [Display(Name = "RTA")]
        public long RTAID { get; set; }

        [Required(ErrorMessage = "Please select the AMC")]
        [Display(Name = "AMC")]
        public long AMCID { get; set; }

        [Required(ErrorMessage = "Please select the Scheme")]
        [Display(Name = "Scheme")]
        public long SchemeID { get; set; }
        [Required(ErrorMessage = "Please select the Scheme")]
        [Display(Name = "Account")]
        public long AccountID { get; set; }

        [Required(ErrorMessage = "Please select the Scheme")]
        [Display(Name = "Account Type")]
        public long AccountTypeID { get; set; }


        [Required(ErrorMessage = "Please enter the PAN")]
        [Display(Name = "PAN")]
        [RegularExpression(@"([A-Z]){5}([0-9]){4}([A-Z]){1}$", ErrorMessage = "Invalid PAN")]
        public string PAN { get; set; }

        [Required(ErrorMessage = "Please select the Client")]
        [Display(Name = "Client")]
        public long ClientID { get; set; }

        [Required(ErrorMessage = "Please select the RTA Table")]
        [Display(Name = "RTA Table")]
        public long RTATableID { get; set; }

        [NotMapped]
        public string RTAName { get; set; }
        [NotMapped]
        public string AMCName { get; set; }
        [NotMapped]
        public string SchemeName { get; set; }

    }
}
