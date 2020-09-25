using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_RTAs")]
    public class RTAs : BaseModel
    {
        public long Id { get; set; }

        [Column("RTAName")]
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters."), MinLength(4, ErrorMessage = "Name cannot be longer than 4 characters.")]
        public string Name { get; set; }

        [Column("RTAAddress")]
        [Required]
        public string Address { get; set; }
    }
}
