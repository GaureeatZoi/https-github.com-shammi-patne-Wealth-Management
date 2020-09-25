using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Client_SecondaryContactDetails")]
    public class ClientSecondaryContact : BaseModel
    {
        public long ID { get; set; }

        public long ClientID { get; set; }

        [Required(ErrorMessage = "Please enter the Name")]
        [Display(Name = "Name")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Please select the Relationship")]
        [Display(Name = "Relationship")]
        public int Relationship { get; set; }

        [Required(ErrorMessage = "Please enter the Mobile Number")]
        [Display(Name = "Mobile Number")]
        [RegularExpression("^([6-9]){1}([0-9]){9}$", ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter the EmailId")]
        [Display(Name = "EmailId")]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please enter the Effective From")]
        [Display(Name = "Effective From")]
        public string EffectiveFrom { get; set; }

        [Required(ErrorMessage = "Please enter the Effective To")]
        [Display(Name = "Effective To")]
        public string EffectiveTo { get; set; }

    }
}
