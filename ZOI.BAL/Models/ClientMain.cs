using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_L3_ClientMain")]
    public class ClientMain : BaseModel
    {
        public long ID { set; get; }


        [Required(ErrorMessage = "Please Select the Family Name")]
        [Display(Name ="Family")]
        public long? FamilyID { set; get;}
       

        
        [Required(ErrorMessage = "Please Select the Title")]
        [Display(Name = "Title")]
        public int? Title { set; get;}
        

        [Required(ErrorMessage = "Please Enter the First Name")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = " Only Alphabets are allowed ")]
        [Display(Name = "Fast Name")]
        public string FirstName { set; get;}

        
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = " Only Alphabets are allowed ")]
        [Display(Name = "Middle Name")]
        public string MiddleName { set; get;}


        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please Enter the Last Name")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = " Only Alphabets are allowed ")]
        public string LastName { set; get;}

        [Display(Name = "Short Name")]
        [Required(ErrorMessage = "Please Enter the Short Name")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = " Only Alphabets are allowed ")]
        public string ShortName { set; get;}


        [Required(ErrorMessage = "Please Select the Gender")]
        public int? Gender { set; get;}


        [Required(ErrorMessage = "Please Enter the DOB")]
        public string DOB { set; get;}

        [Display(Name = "Occupation")]
        public int? OccupationID { set; get;}        
        


        [Required(ErrorMessage = "Please Enter the PAN")]
        [RegularExpression("^([A-Z]){5}([0-9]){4}([A-Z]){1}$", ErrorMessage = "Invalid PAN Number")]
        public string PAN { set; get; }

        [Display(Name = "Introducer")]
        [Required(ErrorMessage = "Please Select the Introducer")]
        public long? IntroducerID { set; get;}

        [Required(ErrorMessage = "Please enter the Mobile Number")]
        [Display(Name = "Mobile Number")]
        [RegularExpression("^([6-9]){1}([0-9]){9}$", ErrorMessage = "Invalid Mobile Number")]
        [Column("MobileNo")]
        public long? MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter the EmailId")]
        [Display(Name = "EmailId")]
        [EmailAddress]
        public string EmailId { get; set; }


        [NotMapped]
        public string FamilyName { set; get; }

        [NotMapped]
        public string ClientName { set; get; }

        [NotMapped]
        public string TitleName { set; get; }

        [NotMapped]
        public string OccupationName { set; get; }

        [NotMapped]
        public string IntroducerName { set; get;}
    
    
    }
}
