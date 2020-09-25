using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Banks")]
    public class Bank  : BaseModel
    {
        public long Id { get; set; }

        [Column("BankName")]
        [Required(ErrorMessage = "Please enter Bank Name")]
        public string Name { get; set; }

        [Column("BankLogo")]
        public string Logo { get; set; }

        [NotMapped]
        public IFormFile LogoFile { get; set; }

        [NotMapped]
        public byte[] LogoFileData { get; set; }
    }
}
