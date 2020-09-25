using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;


namespace ZOI.BAL.Models
{
    [Table("tbl_BankAccountType")]
    public class BankAccountType : Base.BaseModel
    {
        public long Id { get; set; }

        [DisplayName("Account Type Code")]
        [Required(ErrorMessage = "Please enter Account Type Code")]
        public string AcTypeCode { get; set; }

        [DisplayName("Account Type")]
        [Required(ErrorMessage = "Please enter Account Type")]
        public string AccountType { get; set; }


    }
}
