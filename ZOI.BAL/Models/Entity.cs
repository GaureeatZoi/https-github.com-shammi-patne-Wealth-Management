using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ZOI.BAL.Models
{
    [Table("tbl_EntityMaster")]
    public class Entity : Base.BaseModel
    {
        public long ID { get; set; }

        [DisplayName("Entity Type")]
        [Required(ErrorMessage = "Entity Type is required.")]
        public int EntityTypeID { get; set; }

        public string EntityCode { get; set; }

        [DisplayName("Entity Name /First Name")]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Address Line 1")]
        [Required(ErrorMessage = "Address is required.")]
        public string AddressLine1 { get; set; }
        [DisplayName("Address Line 2")]
        public string AddressLine2 { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "Country is required.")]
        public long CountryID { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "State is required.")]
        public long StateID { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "City is required.")]
        public long CityID { get; set; }

        [DisplayName("Pincode")]
        [Required(ErrorMessage = "Pincode is required.")]
        public long Pincode { get; set; }

        [DisplayName("Contact Person")]
        [Required(ErrorMessage = "Contact Person is required.")]
        public string ContactPersonName { get; set; }

        [DisplayName("Contact No")]
        [Required(ErrorMessage = "Contact No. is required.")]
        public string ContactNumber { get; set; }

        [DisplayName("Contact Email")]
        [Required(ErrorMessage = "Contact Email required.")]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [NotMapped]
        public string ManagerName { get; set; }
        [NotMapped]
        public string CityName { get; set; }
        [NotMapped]
        public string StateName { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string EntityTypeName { get; set; }
        [NotMapped]
        public string SebiRegistration { get; set; }
        [NotMapped]
        public string IsHO { get; set; }
        [NotMapped]

        public string LastUpdateOn { get; set; }

    }
}
