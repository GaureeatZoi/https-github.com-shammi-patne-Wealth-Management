using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;


namespace ZOI.BAL.Models
{
    [Table("Tbl_Roles")]
    public class Role : Base.BaseModel
    {
        public int RoleID { get; set; }

        [DisplayName("Role Name")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = " Only Alphabets, Numbers and space are allowed ")]     
        [Required(ErrorMessage = "Role name is required.")]
        public string Name { get; set; }

        [DisplayName("Application Name")]
        [Required(ErrorMessage = "Application name is required.")]
        public long ApplicationId { get; set; }

        [DisplayName("Parent Role ")]
        public int ParentRoleID { get; set; }

        [DisplayName("Role Description")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = " Only Alphabets, Numbers and space are allowed ")]
        [Required(ErrorMessage = "Role Diecription is required.")]
        public string NormalizedName { get; set; }

        [DisplayName("Parent Role Name")]
        public int ParentRoleId { get; set; }
     
        public string ID { get; set; }

        public string ConcurrencyStamp { get; set; }

        [NotMapped]
        public string ParentRoleName { get; set; }

        [NotMapped]
        public string ApplicationName { get; set; }

    }
}
