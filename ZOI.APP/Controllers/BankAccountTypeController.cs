using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;
using OfficeOpenXml;
using System.Linq;


namespace ZOI.APP.Controllers
{
    public class BankAccountTypeController : BaseController
    {
        private readonly IBankAccountTypeService _interface;
        // private readonly IServiceFactory _IServiceFactory;



        JsonResponse resp = new JsonResponse();
        [Obsolete]
        public BankAccountTypeController(IBankAccountTypeService _IBankAccountTypeService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IBankAccountTypeService;
        }

       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            BankAccountTypeViewModel model = new BankAccountTypeViewModel();
            InitAccessModel(model);
            model.bankAccountType.Id = 0;
            return View(model);
        }

       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(long ID)
        {  // returns Update view

            BankAccountTypeViewModel model = new BankAccountTypeViewModel();
            InitAccessModel(model);
            model.bankAccountType = _interface.GetData(ID);
            if (model != null)
            {
                return View("Index", model);
            }
            else
            {
                resp.Message = "";
                return Json(resp);
            }
        }

       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult IsExstis(string Name, long ID)
        { // Check uniqueness of Scheme Plan date
            if (!_interface.IsExsits(Name, ID))
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }

       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(BankAccountTypeViewModel model)
        {  // Inserts or Udates Data
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.bankAccountType));
            }
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);

            }
        }

       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetAll()
        { // gets  listing data
            return Json(_interface.Summary());
        }

       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {  // changes active deacivate status
            return Json(_interface.Deactivate(ID, Status));
        }

       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {   // returns listing view
            BankAccountTypeViewModel model = new BankAccountTypeViewModel();
            InitAccessModel(model);
            return View(model);
        }

        //public IActionResult ExportData(int? exportType)
        //{
        //    //exportType = 2;
        //    //  var list = HttpContext.Session.GetObject<IEnumerable<Dipository>>("ExportData").ToList();




        //    var list = _interface.ListAll().ToList();
        //    var columnHeader = new string[6] { "HolidayDate", "Holiday", "IsSettlementHoliday", "IsTradingHoliday", "Status", "LastUpdatedDate" };

        //    //  Export CSV
        //    if (exportType == 1)
        //    {

        //        var browsercsv = new StringBuilder();
        //        string data = string.Empty;
        //        int count = list.Count();
        //        if (list.Count > 0)
        //        {


        //            list.ForEach(line =>
        //            {
        //                data = list.Count.ToString();
        //                data += "," + line.HolidayDate;
        //                data += "," + line.Holiday;
        //                data += "," + line.IsSettlementHday;
        //                data += "," + line.IsTradingHday;
        //                data += "," + line.IsActiveText;
        //                data += "," + line.LastUpdatedDate;

        //                browsercsv.AppendLine(data);
        //            });
        //        }
        //        else
        //        {
        //            data = "No Data Available";
        //            browsercsv.AppendLine(data);
        //        }
        //        string csvName = $"AllBrowserUsage-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.csv";
        //        byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeader)}\r\n{browsercsv.ToString()}");
        //        return File(buffer, "text/csv", csvName);
        //    }
        //    //  Export Excel
        //    else if (exportType == 2)
        //    {
        //        var stream = new MemoryStream();
        //        if (list.Count() > 0)
        //        {

        //            ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //            using (var package = new ExcelPackage(stream))
        //            {
        //                var workSheet = package.Workbook.Worksheets.Add("Holiday Master Data");
        //                int totalRows = list.Count;
        //                for (var j = 0; j < columnHeader.Length; j++)
        //                {
        //                    workSheet.Cells[1, j + 1].Value = columnHeader[j];
        //                    workSheet.Cells[1, j + 1].Style.Font.Bold = true;
        //                }
        //                int i = 0;
        //                for (int row = 2; row <= totalRows + 1; row++)
        //                {
        //                    workSheet.Cells[row, 1].Value = list[i].HolidayDate;
        //                    workSheet.Cells[row, 2].Value = list[i].Holiday;
        //                    workSheet.Cells[row, 3].Value = list[i].IsSettlementHday;
        //                    workSheet.Cells[row, 4].Value = list[i].IsTradingHday;
        //                    workSheet.Cells[row, 5].Value = list[i].IsActiveText;
        //                    workSheet.Cells[row, 6].Value = list[i].LastUpdatedDate;
        //                    i++;
        //                }
        //                package.Save();
        //            }
        //        }
        //        else
        //        {
        //            using (var package = new ExcelPackage(stream))
        //            {
        //                var workSheet = package.Workbook.Worksheets.Add("Holiday Master Data");
        //                workSheet.Cells[1, 1].Value = "No Data Available";
        //                package.Save();
        //            }
        //        }
        //        stream.Position = 0;
        //        string excelName = $"MasterData-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
        //        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}




    }
}
