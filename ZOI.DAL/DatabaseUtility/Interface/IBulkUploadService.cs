using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IBulkUploadService
    {
        JsonResponse ExcelUpload(BulkUpload model);

        List<SelectListItem> FillRTAs();

        List<SelectListItem> FillFileType();


        JsonResponse GetSheetsRecords(IFormFile file);

        List<BulkUploadData> GetBulkUploadedData();
    }
}
