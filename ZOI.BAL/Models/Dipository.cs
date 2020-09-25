using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ZOI.BAL.Models
{
	[Table("Tbl_Dipositories")]
	public class Dipository : Base.BaseModel
	{
		public long Id { get; set; }

		[Required(ErrorMessage = "Depository Code is required.")]
		[RegularExpression(@"^[A-Z0-9]*$", ErrorMessage = "Only Alphabets A-Z and Numbers  are allowed ")]
		[StringLength(10, MinimumLength = 10)]
		[DisplayName("DP Code")]
		public string DPCode { get; set; }

		[Required(ErrorMessage = "Dipository Name is required.")]
		[StringLength(100, MinimumLength = 10)]
		[DisplayName("DP Name")]
		public string DPName { get; set; }

		[Required(ErrorMessage = "Address is required.")]
		[StringLength(100)]
		[DisplayName("Address Line 1")]
		public string AddressLine1 { get; set; }

		[StringLength(100)]
		[DisplayName("Address Line 2")]
		public string AddressLine2 { get; set; }

		[Required(ErrorMessage = "City is required.")]
		[DisplayName("City")]
		public long CityId { get; set; }

		[Required(ErrorMessage = "State is required.")]
		[DisplayName("State")]
		public long StateId { get; set; }

		[Required(ErrorMessage = "Country is required.")]
		[DisplayName("Country")]
		public long CountryId { get; set; }


		/// <summary>
		/// This will change if any pincode master comes up. 
		/// </summary>
		[Range(100000,999999)]
		[Required(ErrorMessage = "6 Digit Pincode is required.")]
		public long Pincode { get; set; }

		public decimal Longitude { get; set; }
		public decimal Latitude { get; set; }

		[NotMapped]
		public string CityName 		{ get; set; }
		[NotMapped]
		public string StateName	    { get; set; }
		[NotMapped]
		public string CountryName { get; set; }
		[NotMapped]
		public string LastUpdateOn { get; set; }

 
    }
}