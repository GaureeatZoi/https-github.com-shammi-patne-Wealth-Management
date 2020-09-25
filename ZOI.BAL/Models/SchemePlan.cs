using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ZOI.BAL.Models
{
    [Table("tbl_Scheme_Plan")]
   public  class SchemePlan : Base.BaseModel
    {
        public long ID { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please Scheme Plan Name ")]
        public string Name { get; set; }

    }
}
