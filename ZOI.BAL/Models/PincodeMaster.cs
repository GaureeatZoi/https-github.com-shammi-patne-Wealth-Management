using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;


namespace ZOI.BAL.Models
{
	[Table("tbl_pincodes")]
	public class PincodeMaster : Base.BaseModel
	{
		public long ID { get; set; }


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
		[Range(100000, 999999)]
		[Required(ErrorMessage = "6 Digit Pincode is required.")]
		public long PinCode { get; set; }


		[NotMapped]
		public string CityName { get; set; }
		[NotMapped]
		public string StateName { get; set; }
		[NotMapped]
		public string CountryName { get; set; }
	}
}
