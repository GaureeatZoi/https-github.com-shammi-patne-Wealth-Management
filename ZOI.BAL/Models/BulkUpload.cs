using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Models
{
    [Table("tbl_BulkUpload")]
    public class BulkUpload:BaseModel
    {

        public long Id { get; set; }
        [Required(ErrorMessage = "Required")]

        [Display(Name = "RTA's")]
        public long RTAsId { get; set; }
        [ForeignKey("RTAsId")]
        public virtual RTAs RTAs { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "File Type")]
        public long FileTypeId { get; set; }
        [ForeignKey("FileTypeId")]
        public virtual FileType FileType { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Upload File")]
        public string UploadFile { get; set; }

        public string BatchNumber { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> RTAsList { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> FileTypeList { get; set; }
    }
}
