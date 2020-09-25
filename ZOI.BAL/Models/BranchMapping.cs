using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;


namespace ZOI.BAL.Models
{
    [Table("tbl_BranchMapping")]
    public class BranchMapping : Base.BaseModel
    {
        public long ID { get; set; }
        public long EntityID { get; set; }

        [DisplayName("Manager")]
        [Required(ErrorMessage = "Manager is required.")]
        public long ManagerID { get; set; }

        [DisplayName("Is HO")]
        public bool IsHO { get; set; }

    }
}
