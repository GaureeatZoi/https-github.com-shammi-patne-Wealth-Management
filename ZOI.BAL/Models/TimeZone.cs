using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ZOI.BAL.Models
{
    [Table("tbl_timezones")]
    public class TimeZone : Base.BaseModel
    {
        public int ID { get; set; }
        public string TimeZoneName { get; set; }
        public string Description { get; set; }
    }
}
