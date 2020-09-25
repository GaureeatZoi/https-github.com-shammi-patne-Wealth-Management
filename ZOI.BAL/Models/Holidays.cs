using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ZOI.BAL.Models
{
    [Table("Tbl_Holidays")]
   public  class Holidays : Base.BaseModel 
    {
        public long ID { get; set; }
        [DisplayName("Holiday Name")]
        [Required(ErrorMessage = "Holiday name is required.")]
        public string Holiday { get; set; }

        [DisplayName("Holiday Date")]
        [Required(ErrorMessage = "Holiday date is required.")]
        public string  HolidayDate { get; set; }

        [DisplayName("Is Settlement Holiday")]
        public bool IsSettlementHoliday { get; set; }

        [DisplayName("Is Trading Holiday")]
        public bool IsTradingHoliday { get; set; }

        [NotMapped]
        public string IsSettlementHday { get; set; }
        [NotMapped]
        public string IsTradingHday { get; set; }
    }



}
