using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_L1_ClientGroups")]
    public class ClientGroup: BaseModel
    {

        [Column("ID")]
        public long Id { get; set; }

        [Display(Name = "Group Name")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("[A-Za-z  ]*", ErrorMessage = "Please Enter Valid  Group Name")]
        public string  GroupName { get; set; }

        [Display(Name = "Address Line1")]
        [Required(ErrorMessage = "Required")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Required")]
        public long CityID { get; set; }
        //[ForeignKey("CityID")]
        //public virtual City City { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "Required")]
        public long StateID { get; set; }
        //[ForeignKey("StateID")]
        //public virtual State State { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Required")]
        public long CountryID { get; set; }
        //[ForeignKey("CountryID")]
        //public virtual Country Country { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("[0-9]*", ErrorMessage = "Please Enter Valid Pincode")]
        public long? Pincode { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        [NotMapped]
        public string  Countries { get; set; }

        [NotMapped]
        public string States { get; set; }

        [NotMapped]
        public string Cities { get; set; }

 

    }
}
