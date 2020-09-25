using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ZOI.BAL.Models
{
    [Table("tbl_EnumMaster")]
    public  class Enum : Base.BaseModel
    {
        public long ID { get; set; }

        [DisplayName("Enum Type")]
        [Required(ErrorMessage = "Enum Type is required.")]
        public string EnumType { get; set; }

        [DisplayName("Enum Code")]
        [Required(ErrorMessage = "Enum Code is required.")]
        public string EnumCode { get; set; }

        [DisplayName("Enum Value")]
        [Required(ErrorMessage = "Enum Value is required.")]
        public int EnumValue { get; set; }

        [DisplayName("Enum Description")]
        [Required(ErrorMessage = "Enum Description is required.")]
        public string EnumDescription { get; set; }

        [DisplayName("Icon")]
        public string Icons { get; set; }

    }
}
