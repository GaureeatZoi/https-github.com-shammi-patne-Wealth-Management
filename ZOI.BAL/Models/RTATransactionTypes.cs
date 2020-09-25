using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_RTATransactionTypes")]
    public class RTATransactionTypes : BaseModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "Please select the RTA")]
        [Display(Name = "RTA")]
        public long RTAId { get; set; }
        [NotMapped]
        public string RTA { get; set; }

        [Required(ErrorMessage = "Please enter  the RTA Transaction Type")]
        [Display(Name = "RTA Transaction Type")]
        [RegularExpression("^[A-Z]*$", ErrorMessage = "Invalid RTA Transaction Type, Inputs allowed : A-Z")]
        public string RTATransactionType { get; set; }

        [Required(ErrorMessage = "Please enter  the Remarks")]
        [Display(Name = "Remarks")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Remarks should be more than 10 characters ")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Please select the TransactionTypeID")]
        [Display(Name = "Transaction Type")]
        public int TransactionTypeID { get; set; }

        [NotMapped]
        public string TransactionType { get; set; }
        [NotMapped]
        public int row_id { get; set; }

    }
}
