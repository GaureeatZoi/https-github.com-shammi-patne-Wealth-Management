using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using ZOI.BAL;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;
using OfficeOpenXml;
using ZOI.APP.Filters;

namespace ZOI.APP.Controllers
{
    public class CountryController : BaseController
    {
        private readonly ICountryService _CountryService;

        

        [Obsolete]
        private readonly IHostingEnvironment _HostingEnvironment;

        [Obsolete]
        public CountryController( ICountryService countryService, IHostingEnvironment HostingEnvironment, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _CountryService = countryService;
            _HostingEnvironment = HostingEnvironment;
        }

        public Country Country { get; set; }
              
        public  IActionResult IsCountryExists(string name)
        {
            JsonResponse resp = new JsonResponse();
            if (!_CountryService.IsCountryExists(name))
            {
                 resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                 resp.Message = Constants.ControllerMessage.Data_Exsists;

            }
            return Json(resp);

        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            CountryViewModel model = new CountryViewModel();
            InitAccessModel(model);
            return View(model);


        }

      



       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetAll()
        { // gets  listing data


            return Json(_CountryService.Summary());
        }



        [HttpGet]
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IEnumerable<Country> GetAllCountries()
        {
            IEnumerable<Country> CountryData;
           CountryData = _CountryService.ListAll();
            HttpContext.Session.SetObject("ExportData", CountryData);
            return CountryData;
         //   return _CountryService.ListAll();
        }

        [HttpGet]
      // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index(long id)
        {
            CountryViewModel model = new CountryViewModel();
           
            if ( id == 0)
            {
                model.country.Id = 0;
                model.TimeZoneList = _CountryService.GetTimeZoneList();
                return View(model);
            }
            model.country = _CountryService.Find(id);
            if (model.country != null)
            {
                model.country.Id = id;
                model.TimeZoneList = _CountryService.GetTimeZoneList();
                return View(model);
            }
            else
            { return NotFound(); }
            
            
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(CountryViewModel model)
        {
            JsonResponse resp = new JsonResponse();
            if (ModelState.IsValid)
            {
                return Json(_CountryService.AddUpdate(model.country));
            }
            else
            {
                resp.Status = "F";
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {  // changes active deacivate status
            return Json(_CountryService.Deactivate(ID, Status));
        }

        //----------------------------------------------------------------

        public IActionResult ExportData(int? exportType)
        {
            exportType = 1;
            // var list = HttpContext.Session.GetObject<IEnumerable<Country>>("ExportData").ToList();

            var list = _CountryService.ListAll().ToList();

            // Export CSV
            if (exportType == 1)
            {
                var columnHeader = new string[0];
                var browsercsv = new StringBuilder();
                string data = string.Empty;
                //   int count = list.Count();
                if (list.Count > 0)
                {
                    // columnHeader = GetHeaderValues(list.First());
                    // appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    list.ForEach(line =>
                    {
                        data = list.Count.ToString();  
                        data += "," + line.CountryName;
                        data += "," + line.Currency;
                        data += "," + line.CurrencySymbolUnicode;
                        data += "," + line.TimeZone;

                        data += "," + line.IsActive;
                        browsercsv.AppendLine(data);
                    });
                }
                else
                {
                    data = "No Data Available";
                    browsercsv.AppendLine(data);
                }
                string csvName = $"AllBrowserUsage-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.csv";
                byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeader)}\r\n{browsercsv.ToString()}");
                return File(buffer, "text/csv", csvName);
            }
            //Export Excel
            else if (exportType == 2)
            {
                var stream = new MemoryStream();

                var columnHeader = new string[5] { "Country Name", "Currency","Currency Symbol Unicode", "TimeZone", "IsActive" };
                if (list.Count > 0)
                {
                    // columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("State Master Data"); 
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {
                            workSheet.Cells[row, 1].Value = list[i].CountryName;
                            workSheet.Cells[row, 2].Value = list[i].Currency;
                            workSheet.Cells[row, 3].Value = list[i].CurrencySymbolUnicode;
                            workSheet.Cells[row, 4].Value = list[i].TimeZone;
                            workSheet.Cells[row, 5].Value = list[i].IsActive;
                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Country Master Data");
                        workSheet.Cells[1, 1].Value = "No Data Available";
                        package.Save();
                    }
                }
                stream.Position = 0;
                string excelName = $"CountryMasterData-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
