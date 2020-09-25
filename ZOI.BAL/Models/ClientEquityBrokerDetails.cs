using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_Client_EquityBrokerDetails")]
    public class ClientEquityBrokerDetails : BaseModel
    {

        public long ID { get; set; }

        public long ClientID { get; set; }

        [Required(ErrorMessage = "Please select the Broker")]
        [Display(Name = "Broker")]
        public long BrokerID { get; set; }

        [Required(ErrorMessage = "Please enter the Broker UCC")]
        [Display(Name = "Broker UCC")]
        [RegularExpression("^([A-Z]){5}([0-9]){5}$", ErrorMessage = "Invalid Broker UCC")]
        public string BrokerUCC { get; set; }


        [Required(ErrorMessage = "Please enter the Effective From")]
        [Display(Name = "Effective From")]
        public string EffectiveFrom { get; set; }

        [Required(ErrorMessage = "Please enter the Effective To")]
        [Display(Name = "Effective To")]
        public string EffectiveTo { get; set; }

    }
}
