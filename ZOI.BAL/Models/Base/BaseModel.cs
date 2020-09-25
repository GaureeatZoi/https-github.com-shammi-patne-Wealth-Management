using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZOI.BAL.Models.Base
{

    public class BaseModel
    {
        //Base Model consist fields which are common for all the table


        public long CreatedBy { get; set; }
        
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } 
       
        public long? ModifiedBy { get; set; }
        
        public DateTime? ModifiedOn { get; set; }

        [NotMapped]
        public string   LastUpdatedDate { get; set; }
        [NotMapped]
        public string IsActiveText { get; set; }
       

    }
}
