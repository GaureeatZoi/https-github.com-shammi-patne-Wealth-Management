using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Client_PersonlaDetails")]
    public class ClientPersonal : BaseModel
    {
        public long ID { get; set; }

        public long ClientID { get; set; }

        [Required(ErrorMessage = "Please select the  Marital Status")]
        [Display(Name = "Marital Status")]
        public int MaritalStatusID { get; set; }

        [Required(ErrorMessage = "Please enter the Mother First Name")]
        [Display(Name = "Mother First Name")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Mother First Name field allowed characters only")]
        public string MotherFirstName { get; set; }

        [Required(ErrorMessage = "Please enter the Mother Last Name")]
        [Display(Name = "Mother Last Name")]
        public string MotherLastName { get; set; }

        [Required(ErrorMessage = "Please enter the Spouse/Father First Name")]
        [Display(Name = "Spouse/Father First Name")]
        public string SpouseOrFatherFirstName { get; set; }

        [Required(ErrorMessage = "Please enter the Spouse/Father Last Name")]
        [Display(Name = "Spouse/Father Last Name")]
        public string SpouseOrFatherLastName { get; set; }

        [Required(ErrorMessage = "Please enter the Maiden Last Name")]
        [Display(Name = "Maiden First Name")]
        public string MaidenFirstName { get; set; }

        [Required(ErrorMessage = "Please enter the Maiden Last Name")]
        [Display(Name = "Maiden Last Name")]
        public string MaidenLastName { get; set; }

        [Required(ErrorMessage = "Please select the Annual Income")]
        [Display(Name = "Annual Income")]
        public int AnnualIncomeID { get; set; }

        [Required(ErrorMessage = "Please select the Trading Experience")]
        [Display(Name = "Trading Experience")]
        public int TradingExperienceID { get; set; }

        [Required(ErrorMessage = "Please select the Commodity Trade Classification")]
        [Display(Name = "Commodity Trade Classification")]
        public int CommodityTradeClassificationID { get; set; }


        [Display(Name = "Is Political Experience")]
        public bool IsPoliticalExperienceID { get; set; }

        [Required(ErrorMessage = "Please select the Citizenship")]
        [Display(Name = "Citizenship")]
        public int CitizenshipID { get; set; }

        [Required(ErrorMessage = "Please select the Residential Status")]
        [Display(Name = "Residential Status")]
        public int ResidentialStatusID { get; set; }

        [Required(ErrorMessage = "Please select Education")]
        [Display(Name = "Education")]
        public int EducationID { get; set; }

    }
}
