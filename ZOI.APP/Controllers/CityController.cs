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
    public class CityController : BaseController
    {
       
        private readonly ICityService _CityService;
        
        [Obsolete]
        public CityController(ICityService cityService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _CityService = cityService;
        }

        public City City { get; set; }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            CityViewModel model = new CityViewModel();
            InitAccessModel(model);  
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetAll()
        { // gets  listing data


            return Json(_CityService.Summary());
        }

        [HttpGet]
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IEnumerable<City> GetAllCities()
        {  
            // returns city listings.
            IEnumerable<City> CityData = _CityService.ListAll();
            HttpContext.Session.SetObject("CityMasterData", CityData);
            return CityData;
        }

        [HttpGet]
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index(long id)
        {  
            // Returns AddEditCity view for update or insert
            CityViewModel model = new CityViewModel();
            InitAccessModel(model);
            model.TierList = _CityService.GetCityTierList();
            model.StateList = _CityService.InitCityView();
            if ( id != 0)
            {  
            
            model.city = _CityService.Find(id);
            model.StateList = _CityService.InitCityView();
            }
            if (model.city == null)
            {  
                return NotFound();
            }
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(CityViewModel model)
        {    
            // Updates or inserts city
            JsonResponse resp = new JsonResponse();
            if (ModelState.IsValid)
            { 
                return Json(_CityService.AddUpdate(model.city));
            }
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {  // changes active deacivate status
            return Json(_CityService.Deactivate(ID, Status));
        }

        public IActionResult ExportData(int? exportType)
        {  // Exports data in csv or excell format
            exportType = 2;
            // var list = HttpContext.Session.GetObject<IEnumerable<City>>("ExportData").ToList();

            var list = _CityService.ListAll().ToList();
            var columnHeader = new string[4] { "City Name", "State Name", "Country Name", "IsActive" };
            // Export CSV
            if (exportType == 1)
            {
                //var columnHeader = new string[0];
                var browsercsv = new StringBuilder();
                string data = string.Empty;
                //   int count = list.Count();
                if (list.Count > 0)
                {
                    // columnHeader = GetHeaderValues(list.First());
                    list.ForEach(line =>
                    {
                        data = list.Count.ToString();
                        data += "," + line.CityName;
                        data += "," + line.StateName;

                        data += "," + line.CountryName;
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

                
                if (list.Count > 0)
                {
                    // columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    using (var package = new ExcelPackage(stream))
                    {   var workSheet = package.Workbook.Worksheets.Add("State Master Data");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {   workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {   workSheet.Cells[row, 1].Value = list[i].CityName;
                            workSheet.Cells[row, 2].Value = list[i].StateName;
                            workSheet.Cells[row, 3].Value = list[i].CountryName;
                            workSheet.Cells[row, 4].Value = list[i].IsActive;
                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {   using (var package = new ExcelPackage(stream))
                    {  var workSheet = package.Workbook.Worksheets.Add("City Master Data");
                        workSheet.Cells[1, 1].Value = "No Data Available";
                        package.Save();
                    }
                }
                stream.Position = 0;
                string excelName = $"CityMasterData-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {  return NotFound();
            }
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult IsCityExists(string name)
        {   // Validates City name for uniqueness
            JsonResponse resp = new JsonResponse();
            if (!_CityService.IsCityExists(name))
            {
                 resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                 resp.Message = Constants.ControllerMessage.Data_Exsists;

            }
            return Json(resp);
        }
    
    }
}
