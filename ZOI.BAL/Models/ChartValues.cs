using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZOI.BAL.Models
{
    [Table("tbl_TopScheme", Schema = "dbo")]
    public class ChartValues
    { 

        public string Name { get; set; }

        public int Percentage { get; set; }

        public List<ChartValues> ChartList { get; set; }
    }
}
