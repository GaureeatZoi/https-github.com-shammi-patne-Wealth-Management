using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_L2_ClientFamilies")]
    public class ClientFamily:BaseModel
    {

        [Column("ID")]
        public long Id { get; set; }

        [Display(Name = "Family Name")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("[A-Za-z  ]*", ErrorMessage = "Please Enter Valid  Family Name")]
        public string FamilyName { get; set; }

        [Display(Name = "Family Short Name")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression("[A-Za-z  ]*", ErrorMessage = "Please Enter Valid  Family Short Name")]
        public string FamilyShortName { get; set; }

        [Display(Name = "Group")]
        [Required(ErrorMessage = "Required")]
        public long GroupID { get; set; }
        [ForeignKey("GroupID")]
        public virtual ClientGroup ClientGroup { get; set; }

        [Display(Name = "Address Line1")]
        [Required(ErrorMessage = "Required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Address Field must be at least 10 characters.")]
        [RegularExpression("^[a-zA-Z -/,.]*$", ErrorMessage = "A-Za-z0-9 .,-/ Address Field Allowed these characters only")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line2")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Address Field must be at least 10 characters.")]
        [RegularExpression("^[a-zA-Z -/,.]*$", ErrorMessage = "A-Za-z0-9 .,-/ Address Field Allowed these characters only")]
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
        [RegularExpression("^([0-9]){6}$", ErrorMessage = "Invalid Pincode")]
        public long? Pincode { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        [NotMapped]
        public string Groups { get; set; }

        [NotMapped]
        public string Countries { get; set; }

        [NotMapped]
        public string States { get; set; }

        [NotMapped]
        public string Cities { get; set; }

        
    }
}
