using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZOI.BAL.Models
{
    [Table("tbl_SchemeType")]
    public class SchemeType : Base.BaseModel
    {
        public long ID { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please Enter Scheme Type Name")]
        public string Name { get; set; }

    }
}
