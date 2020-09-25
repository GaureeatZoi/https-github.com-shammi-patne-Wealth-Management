using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ZOI.BAL.Models
{
    [Table("Tbl_EmployeeMaster")]
    public class Employee : Base.BaseModel
    {
        public int ID { get; set; }

        [DisplayName("Employee Code")]
        [Required(ErrorMessage = "Employee Code is required.")]
        public string EmployeeCode { get; set; }

        [DisplayName("First Name")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = " Only Alphabelts are allowed ")]
        [Required(ErrorMessage = "First name is required.")]
        public string FName { get; set; }

        [DisplayName("Middle Name")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = " Only Alphabelts are allowed ")]
       
        public string MName { get; set; }

        [DisplayName("Last Name")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = " Only Alphabelts are allowed ")]
        [Required(ErrorMessage = "Last name is required.")]
        public string LName { get; set; }

        [DisplayName("Gender")]
        [Required(ErrorMessage = "Gender is required.")]
        public int Gender { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [DisplayName("Mobile No")]
        [RegularExpression(@"^[0-9+]*$", ErrorMessage = " Only numbers are allowed ")]
        [Required(ErrorMessage = "Phone No. is required.")]
        public string MobileNo { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentID { get; set; }

        [DisplayName("Sub Department")]
        [Required(ErrorMessage = "Sub Department is required")]
        public int SubDepartmentID { get; set; }

        [DisplayName("Reporting To")]
        [Required(ErrorMessage = "Reporting to is required")]
        public int ReportingTo { get; set; }

        [DisplayName("DOB")]
        [Required(ErrorMessage = "DOB is required")]
      
        public DateTime DOB { get; set; } 
        

        [DisplayName("PAN")]
        [RegularExpression(@"([A-Z]){5}([0-9]){4}([A-Z]){1}$", ErrorMessage = "Invalid PAN")]
        [Required(ErrorMessage = "PAN is required")]
        public string PAN { get; set; }

        [DisplayName("Certification No")]
        public string CertificationNo { get; set; }

        [DisplayName("Designation")]
        [Required(ErrorMessage = "Designation is required")]
        public int DesignationId { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }

        [NotMapped]
        public string GenderName { get; set; }
        [NotMapped]
        public string EmployeeName { get; set; }
        [NotMapped]
        public string ReportingName { get; set; }
        [NotMapped]
        public string Department { get; set; }
        [NotMapped]
        public string SubDepartment { get; set; }
        [NotMapped]
        public string Designation { get; set; }
        [NotMapped]
        public string Role { get; set; }
        [NotMapped]
        public string LastUpdateOn { get; set; }

    }
}
