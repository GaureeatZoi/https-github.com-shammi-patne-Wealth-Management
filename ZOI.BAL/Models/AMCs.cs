using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_AMCs")]
    public class AMCs : BaseModel
    {
        public long Id { get; set; }

        [Column("AMCName")]
        [DisplayName("Name")]
        [Required(ErrorMessage ="Please Enter AMC Name")]
        public string Name { get; set; }

        [Column("AMCAddress")]
        [DisplayName("Address")]
        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }

        [Required(ErrorMessage ="Please select PNG Logo")]
        public string Logo { get; set; }        

        [Column("RTAID")]
        [DisplayName("RTA")]
        [Required(ErrorMessage = "Please Select RTA")]
        public long RTAId { get; set; }


        [NotMapped]
        public IFormFile LogoFile { get; set; }

        [NotMapped]
        public byte[] LogoFileData { get; set; }       

        [NotMapped]
        public string RTAName { get; set; }

    }
}
