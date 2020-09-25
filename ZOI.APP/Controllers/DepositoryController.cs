using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;
using System.Linq;
using OfficeOpenXml;
using System.Reflection;

namespace ZOI.APP.Controllers
{
    public class DepositoryController : BaseController
    {
        private readonly IDepositoryService _interface;

        JsonResponse resp = new JsonResponse();
        
        public DepositoryController(IDepositoryService _IDepositoryService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IDepositoryService;


        }

        //[TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {  // returns Add View

            DepositoryViewModel model = new DepositoryViewModel();
            InitAccessModel(model);
            model.Depository.CountryId = 1;
            model.Depository.StateId = 1;
            model.CountryList = _interface.GetCountryList();
            model.StateList = _interface.GetStateList(model.Depository.CountryId);
            model.CityList = _interface.GetCityList(model.Depository.StateId);
            return View(model);
        }

        //[TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(long ID)
        { // returns Update View
            DepositoryViewModel model = new DepositoryViewModel();
           InitAccessModel(model);
            model.Depository = _interface.GetData(ID); 
            if (model != null)
            {
                model.Depository = _interface.GetData(ID);
            };
            if (model.Depository != null)
            {
                model.CountryList = _interface.GetCountryList();
                model.StateList = _interface.GetStateList(model.Depository.CountryId);
                model.CityList = _interface.GetCityList(model.Depository.StateId);
                return View("Index", model);
            }
            else
            {
                return Json(resp);
            }
        }

        //[TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult IsExstis(string name, long ID)
        {  // check uniqueness of dp name
            if (!_interface.IsExsits(name, ID))
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }

        //[TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(DepositoryViewModel model)
        {  //  adds or updates depositorydata
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.Depository));
            }
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }
        }


        //[TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetAll()
        {  // returnss depositorylist data
            return Json(_interface.Summary());
        }

        

        public IActionResult ExportData(int? exportType)
        {
            //exportType = 2;
            //  var list = HttpContext.Session.GetObject<IEnumerable<Depository>>("ExportData").ToList();




            var list = _interface.ListAll().ToList();
             var columnHeader = new string[11] { "DPCode", "DP Name", "AddressLine1", "AddressLine1", "City", "Pincode", "State", "Country", "Latitude", "Longitude", "IsActive" };

            //  Export CSV
            if (exportType == 1)
            {
               
                var browsercsv = new StringBuilder();
                string data = string.Empty;
                int count = list.Count();
                if (list.Count > 0)
                {
               
                    // appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    list.ForEach(line =>
                    {
                        data = list.Count.ToString();
                        data += "," + line.DPCode;
                        data += "," + line.DPName;
                        data += "," + line.AddressLine1;
                        data += "," + line.AddressLine2;
                        data += "," + line.CityName;
                        data += "," + line.Pincode;
                        data += "," + line.StateName;
                        data += "," + line.CountryName;
                        data += "," + line.Latitude;
                        data += "," + line.Longitude;
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
            //  Export Excel
            else if (exportType == 2)
            {
                var stream = new MemoryStream();
                if (list.Count() > 0)
                {
               
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Depository Master Data");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {
                            workSheet.Cells[row, 1].Value = list[i].DPCode;
                            workSheet.Cells[row, 2].Value = list[i].DPName;
                            workSheet.Cells[row, 3].Value = list[i].AddressLine1;
                            workSheet.Cells[row, 4].Value = list[i].AddressLine2;
                            workSheet.Cells[row, 5].Value = list[i].CityName;
                            workSheet.Cells[row, 6].Value = list[i].Pincode;
                            workSheet.Cells[row, 7].Value = list[i].StateName;
                            workSheet.Cells[row, 8].Value = list[i].CountryName;
                            workSheet.Cells[row, 9].Value = list[i].Latitude;
                            workSheet.Cells[row, 10].Value = list[i].Longitude;
                            workSheet.Cells[row, 11].Value = list[i].IsActive;

                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Depository Master Data");
                        workSheet.Cells[1, 1].Value = "No Data Available";
                        package.Save();
                    }
                }
                stream.Position = 0;
                string excelName = $"MasterData-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return NotFound();
            }
        }






        public IActionResult ChangeStatus(long ID, bool Status)
        {  // changes active / deactive status
            return Json(_interface.Deactivate(ID, Status));
        }


        //[TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            DepositoryViewModel model = new DepositoryViewModel();
            InitAccessModel(model);
            return View(model);


        }
        public IActionResult GetCityList(long StateID)
        { // city dropdown
            return Json(_interface.GetCityList(StateID));
        }

        public IActionResult GetStateList(long CountryID)
        {   // state dropdown
            return Json(_interface.GetStateList(CountryID));
        }



         }
}
