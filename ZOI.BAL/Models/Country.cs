using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ZOI.BAL.Models
{
    [Table("Tbl_Countries")]
    public class Country : Base.BaseModel
    {
       
        public long Id { get; set; }

        [Required (ErrorMessage = "Country name is required.")]
        [DisplayName("Country")]
        public string CountryName { get; set; }

        [Required (ErrorMessage = "Currency is required.")]
        [DisplayName("Currency")]
        public string Currency { get; set; }


        [RegularExpression(@"^[a-zA-Z0-9#&;]*$", ErrorMessage = " Only Alphabelts, Numbers ,#,& and ; are allowed ")]
        [StringLength(7, MinimumLength = 7)]
        [DisplayName("Currency Symbol Unicode")]
        [Required  (ErrorMessage = "Currency Symbol is required.")]
        public string CurrencySymbolUnicode { get; set; }
       
        [Required   (ErrorMessage = "Time Zone is required.")]
        [DisplayName("Time Zone")]
        public int TimeZone { get; set; }

        [Required(ErrorMessage = "Country code is required.")]
        [Range(1,  9999, ErrorMessage = "Enter valid country code")]
        [DisplayName("Country Code")]
        public int CountryCode { get; set; }

        [NotMapped]
        public string TimeZoneName { get; set; }
    }

  
}
