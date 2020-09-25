using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ZOI.BAL.Models
{
    [Table("tbl_Cities")]
   public  class City : Base.BaseModel
    {
        public long Id { get; set; }
        [DisplayName("Country")]
        public long? CountryId { get; set; }

        [Required (ErrorMessage = "State name is required.")]
        [DisplayName("State")]
        public long? StateId { get; set; }

        [Required (ErrorMessage = "City name is required.")]
     
        [DisplayName("City")]
        [StringLength(50, MinimumLength = 4)]
        public string CityName { get; set; }
        //new field
        public int Tier { get; set; }
        [NotMapped]
        public string StateName { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string Status { get; set; }

        [NotMapped]
        public string CityTier { get; set; }
     
    }
}
