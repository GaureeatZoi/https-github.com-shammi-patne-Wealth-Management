using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using ZOI.BAL.Models;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class BulkUploadController : BaseController
    {
        private readonly IBulkUploadService _bulkUploadService;
        public BulkUploadController(IBulkUploadService bulkUploadService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _bulkUploadService = bulkUploadService;
        }
        public IActionResult Index()
        {
            BulkUploadViewModel model = new BulkUploadViewModel();
            model.bulkUpload.RTAsList = _bulkUploadService.FillRTAs();
            model.bulkUpload.FileTypeList = _bulkUploadService.FillFileType();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Upload Excel file to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public JsonResult ImportExcelFile(BulkUpload model)
        {
            var request = _bulkUploadService.ExcelUpload(model);
            return Json(request);
        }

        /// <summary>
        /// Get already uploaded file records
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public JsonResult GetUploadedFileDetails(IFormFile file)
        {
            var request = _bulkUploadService.GetSheetsRecords(file);
            return Json(request);
        }
    
        /// <summary>
        /// Get Header Name from Object
        /// </summary>
        /// <param name="modelClass"></param>
        /// <returns></returns>
        public string[] GetHeaderValues(object modelClass)
        {
            PropertyInfo[] listPI = modelClass.GetType().GetProperties();
            List<string> headerValues = new List<string>();
            string displayName = string.Empty;
            foreach (PropertyInfo pi in listPI)
            {
                if (pi != null && pi.Name != "ListBulkData")
                {
                    displayName = pi.Name;
                    headerValues.Add(displayName);
                }
            }
            return headerValues.ToArray();
        }

        /// <summary>
        /// Export Excel file
        /// </summary>
        /// <returns></returns>
        public IActionResult ExportResponseFile()
        {
            BulkUploadData model = new BulkUploadData();
            model.ListBulkData = _bulkUploadService.GetBulkUploadedData();
            var stream = new MemoryStream();
            var columnHeader = new String[0];
            if (model.ListBulkData.Count() > 0)
            {
                columnHeader = GetHeaderValues(model.ListBulkData.First());
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("CAMS Transaction");
                    int totalRows = model.ListBulkData.Count();
                    for (var j = 0; j < columnHeader.Length; j++)
                    {
                        workSheet.Cells[1, j + 1].Value = columnHeader[j];
                        workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                    }
                    int i = 0;
                    for (int row = 2; row <= totalRows + 1; row++)
                    {
                        workSheet.Cells[row, 1].Value = model.ListBulkData[i].amc_code;
                        workSheet.Cells[row, 2].Value = model.ListBulkData[i].folio_no;
                        workSheet.Cells[row, 3].Value = model.ListBulkData[i].prodcode;
                        workSheet.Cells[row, 4].Value = model.ListBulkData[i].scheme;
                        workSheet.Cells[row, 5].Value = model.ListBulkData[i].inv_name;
                        workSheet.Cells[row, 6].Value = model.ListBulkData[i].trxntype;
                        workSheet.Cells[row, 7].Value = model.ListBulkData[i].trxnno;
                        workSheet.Cells[row, 8].Value = model.ListBulkData[i].trxnmode;
                        workSheet.Cells[row, 9].Value = model.ListBulkData[i].trxnstat;
                        workSheet.Cells[row, 10].Value = model.ListBulkData[i].usercode;
                        workSheet.Cells[row, 11].Value = model.ListBulkData[i].usrtrxno;
                        workSheet.Cells[row, 12].Value = model.ListBulkData[i].traddate;
                        workSheet.Cells[row, 13].Value = model.ListBulkData[i].postdate;
                        workSheet.Cells[row, 14].Value = model.ListBulkData[i].purprice;
                        workSheet.Cells[row, 15].Value = model.ListBulkData[i].units;
                        workSheet.Cells[row, 16].Value = model.ListBulkData[i].amount;
                        workSheet.Cells[row, 17].Value = model.ListBulkData[i].brokcode;
                        workSheet.Cells[row, 18].Value = model.ListBulkData[i].subbrok;
                        workSheet.Cells[row, 19].Value = model.ListBulkData[i].brokperc;
                        workSheet.Cells[row, 20].Value = model.ListBulkData[i].brokcomm;
                        workSheet.Cells[row, 21].Value = model.ListBulkData[i].altfolio;
                        workSheet.Cells[row, 22].Value = model.ListBulkData[i].rep_date;
                        workSheet.Cells[row, 24].Value = model.ListBulkData[i].time1;
                        workSheet.Cells[row, 25].Value = model.ListBulkData[i].trxnsubtyp;
                        workSheet.Cells[row, 26].Value = model.ListBulkData[i].application_no;

                        workSheet.Cells[row, 27].Value = model.ListBulkData[i].trxn_nature;
                        workSheet.Cells[row, 28].Value = model.ListBulkData[i].tax;
                        workSheet.Cells[row, 29].Value = model.ListBulkData[i].total_tax;
                        workSheet.Cells[row, 30].Value = model.ListBulkData[i].te_15h;
                        workSheet.Cells[row, 31].Value = model.ListBulkData[i].micr_no;
                        workSheet.Cells[row, 32].Value = model.ListBulkData[i].remarks;
                        workSheet.Cells[row, 33].Value = model.ListBulkData[i].swflag;
                        workSheet.Cells[row, 34].Value = model.ListBulkData[i].old_folio;
                        workSheet.Cells[row, 35].Value = model.ListBulkData[i].seq_no;
                        workSheet.Cells[row, 36].Value = model.ListBulkData[i].reinvest_flag;
                        workSheet.Cells[row, 37].Value = model.ListBulkData[i].mult_brok;
                        workSheet.Cells[row, 38].Value = model.ListBulkData[i].stt;
                        workSheet.Cells[row, 39].Value = model.ListBulkData[i].location;
                        workSheet.Cells[row, 40].Value = model.ListBulkData[i].scheme_type;
                        workSheet.Cells[row, 41].Value = model.ListBulkData[i].tax_status;
                        workSheet.Cells[row, 42].Value = model.ListBulkData[i].load;
                        workSheet.Cells[row, 43].Value = model.ListBulkData[i].scanrefno;
                        workSheet.Cells[row, 44].Value = model.ListBulkData[i].pan;
                        workSheet.Cells[row, 45].Value = model.ListBulkData[i].inv_iin;
                        workSheet.Cells[row, 46].Value = model.ListBulkData[i].targ_src_scheme;
                        workSheet.Cells[row, 47].Value = model.ListBulkData[i].trxn_type_flag;
                        workSheet.Cells[row, 48].Value = model.ListBulkData[i].ticob_trtype;
                        workSheet.Cells[row, 49].Value = model.ListBulkData[i].ticob_trno;
                        workSheet.Cells[row, 50].Value = model.ListBulkData[i].ticob_posted_date;
                        workSheet.Cells[row, 51].Value = model.ListBulkData[i].dp_id;

                        workSheet.Cells[row, 52].Value = model.ListBulkData[i].trxn_charges;
                        workSheet.Cells[row, 53].Value = model.ListBulkData[i].eligib_amt;
                        workSheet.Cells[row, 54].Value = model.ListBulkData[i].src_of_txn;
                        workSheet.Cells[row, 55].Value = model.ListBulkData[i].trxn_suffix;
                        workSheet.Cells[row, 56].Value = model.ListBulkData[i].siptrxnno;
                        workSheet.Cells[row, 57].Value = model.ListBulkData[i].ter_location;
                        workSheet.Cells[row, 58].Value = model.ListBulkData[i].euin;
                        workSheet.Cells[row, 59].Value = model.ListBulkData[i].euin_valid;
                        workSheet.Cells[row, 60].Value = model.ListBulkData[i].euin_opted;
                        workSheet.Cells[row, 70].Value = model.ListBulkData[i].sub_brk_arn;
                        workSheet.Cells[row, 61].Value = model.ListBulkData[i].exch_dc_flag;
                        workSheet.Cells[row, 62].Value = model.ListBulkData[i].src_brk_code;
                        workSheet.Cells[row, 63].Value = model.ListBulkData[i].sys_regn_date;
                        workSheet.Cells[row, 64].Value = model.ListBulkData[i].ac_no;
                        workSheet.Cells[row, 65].Value = model.ListBulkData[i].bank_name;
                        workSheet.Cells[row, 66].Value = model.ListBulkData[i].reversal_code;
                        workSheet.Cells[row, 67].Value = model.ListBulkData[i].exchange_flag;
                        workSheet.Cells[row, 68].Value = model.ListBulkData[i].ca_initiated_date;
                        workSheet.Cells[row, 69].Value = model.ListBulkData[i].gst_state_code;
                        workSheet.Cells[row, 70].Value = model.ListBulkData[i].igst_amount;
                        workSheet.Cells[row, 71].Value = model.ListBulkData[i].cgst_amount;
                        workSheet.Cells[row, 72].Value = model.ListBulkData[i].sgst_amount;
                        workSheet.Cells[row, 73].Value = model.ListBulkData[i].rev_remark;
                        workSheet.Cells[row, 74].Value = model.ListBulkData[i].original_trxnno;
                        i++;
                    }
                    package.Save();
                }
            }
            else
            {
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("CAMS Transaction");
                    workSheet.Cells[1, 1].Value = "No Data Available";
                    package.Save();
                }
            }
            stream.Position = 0;
            string excelName = $"CAMSTransaction-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

    }
}
