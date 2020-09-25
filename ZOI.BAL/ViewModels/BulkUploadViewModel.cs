using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class BulkUploadViewModel : BaseViewModel
    {
        public BulkUploadViewModel()
        {
            this.bulkUpload = new BulkUpload();
        }

        public BulkUpload bulkUpload { get; set; }
    }
}
