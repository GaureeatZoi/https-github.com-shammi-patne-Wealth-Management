using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;


namespace ZOI.BAL.Models
{
    [Table("Tbl_States")]
    public class State : Base.BaseModel
    {
        public long Id { get; set; }

        [Required (ErrorMessage = "Country is required.")]
        [DisplayName("Country")]
        public long  CountryId { get; set; }

        [Required (ErrorMessage = "State name is required.")]
        [StringLength(50,  MinimumLength = 3)]
        [DisplayName("State")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "State code is required.")]
        [StringLength(5, MinimumLength = 2)]
        [DisplayName("State Code")]
        public string StateCode { get; set; }

        [Required(ErrorMessage = "GST State code is required.")]
        [Range(1, 99, ErrorMessage = "Enter two digit unique value")]

        [DisplayName("GST State Code")]
        public int GSTStateCode { get; set; }

        [DisplayName("Is Union Territory")]
        public bool IsUnionTerritory { get; set; }

        [NotMapped]
        public string CountryName { get; set; }
  

    }
}
