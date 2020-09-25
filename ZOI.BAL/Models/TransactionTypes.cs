using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_transactiontypes")]
    public class TransactionTypes : BaseModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage ="Please enter the Transaction Type")]
        [Column("Transaction Type")]
        public string Type { get; set; }
        
        [Required(ErrorMessage ="Please select the Transaction Nature")]
        [Column("Transaction Nature")]
        public string Nature { get; set; }

    }
}
