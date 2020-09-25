using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;


namespace ZOI.BAL.Models
{
    [Table("tbl_SchemeMaster")]
    public class Schema : Base.BaseModel
    {
        //Scheme section
        public long ID { get; set; }

        [Display(Name = "Scheme Code")]
        [Required(ErrorMessage = "Please enter Scheme Code")]
        [RegularExpression("[A-Za-z0-9 ]*", ErrorMessage = "Invalid Scheme Code")]
        public string SchemeCode { get; set; }

        [Display(Name = "Select RTA")]
        [Required(ErrorMessage = "Please select RTA ")]
        public int RTAID { get; set; }

        [Display(Name = "Scheme Name")]
        [RegularExpression("[A-Za-z0-9 ]*", ErrorMessage = "Invalid Scheme Name")]
        [Required(ErrorMessage = "Please enter Scheme Name")]
        public string SchemeName { get; set; }

        [Display(Name = "Old Scheme Name")]
        [RegularExpression("[A-Za-z0-9 ]*", ErrorMessage = "Invalid Old Scheme Name")]
        [Required(ErrorMessage = "Please enter Old Scheme Name")]
        public string OldSchemeName { get; set; }

        [Display(Name = "Short Name")]
        [RegularExpression("[A-Za-z0-9 ]*", ErrorMessage = "Invalid Short Name")]
        [Required(ErrorMessage = "Please enter Short Name")]
        public string ShortName { get; set; }

        [Display(Name = "Select AMC")]
        [Required(ErrorMessage = "Please select AMC ")]
        public int AMCID { get; set; }

        [Display(Name = "Scheme Type")]
        [Required(ErrorMessage = "Please select Scheme Type ")]
        [Column(name: "SchemeType")]
        public int SchemeTypeID { get; set; }

        [Display(Name = "Plan ID")]
        [Required(ErrorMessage = "Please select Plan ")]
        public int PlanID { get; set; }

        [Display(Name = "RTA Code")]
        [RegularExpression("[A-Za-z0-9 ]*", ErrorMessage = "Invalid RTA Code")]
        [Required(ErrorMessage = "Please enter RTA Code")]
        public string RTACode { get; set; }

        [Display(Name = "RTA Prod Code")]
        [RegularExpression("[A-Za-z0-9 ]*", ErrorMessage = "Invalid RTA ProdCode")]
        [Required(ErrorMessage = "Please enter RTA Prod Code")]
        public string RTAProdCode { get; set; }

        [Display(Name = "Asset Class")]
        [Required(ErrorMessage = "Please select Asset Class")]
        public int AssetClassId { get; set; }

        [Display(Name = "Sub Asset Class")]
        [Required(ErrorMessage = "Please select Sub Asset Class")]
        public int SubAssetClassId { get; set; }

        [Required(ErrorMessage = "Please select WEF")]
        public DateTime? WEF { get; set; }

       



        [NotMapped]
        public string  RtaName { get; set; }
        [NotMapped]
        public string SchemeTypeName { get; set; }
        [NotMapped]
        public string AmcName { get; set; }
        [NotMapped]
        public string PlanName { get; set; }
        [NotMapped]
        public string OptionTypeName { get; set; }
    }
}
