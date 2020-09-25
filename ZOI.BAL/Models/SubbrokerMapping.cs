using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ZOI.BAL.Models
{
    [Table("tbl_SubbrokerMapping")]
    public class SubbrokerMapping :Base.BaseModel
    {
        public long ID { get; set; }
        public long EntityID { get; set; }

        [DisplayName("SEBI Registration No")]
        [Required(ErrorMessage = "SEBI Registration No is required.")]
        public string SEBIRegistrationNo { get; set; }

        [DisplayName("Registration Date")]
        [Required(ErrorMessage = "Registration Date is required.")]
        public DateTime SEBIRegistrationDate { get; set; }

    }
}
