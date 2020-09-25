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
    public class EntityController : BaseController
    {
        private readonly IEntityService _interface;

        JsonResponse resp = new JsonResponse();
        [Obsolete]
        public EntityController(IEntityService _IEntityService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IEntityService;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(EntityViewModel model)
        {  
            return Json(_interface.AddUpdate( model));
            
        }

        public IActionResult IsExstis(string Name, long ID)
        { // Check uniqueness of Entity Name
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

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetAll()
        { // gets entity listing data

          
            return Json(_interface.Summary());
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {   // returns listing view
           EntityViewModel model = new EntityViewModel();
            InitAccessModel(model);
            return View(model);
        }

        public IActionResult ChangeStatus(long ID, bool Status)
        {  // changes active deacivate status
            return Json(_interface.Deactivate(ID, Status));
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        { EntityViewModel model = new EntityViewModel();
            InitAccessModel(model);
            model.subbrokerMapping.SEBIRegistrationDate = DateTime.Today;
            //model.entity.Code = "FR0001";
            model.Country = _interface.GetCountryList();
           model.State = _interface.GetStateList(1);
            model.City = _interface.GetCityList(1);
            model.EntityType = _interface.GetEntityTypeList();
            model.Manager = _interface.GetManagerList();

            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(long ID)
        {  // returns Update view

            EntityViewModel model = new EntityViewModel();
            InitAccessModel(model);
            model = _interface.GetData(ID);
            if (model != null)
            {
               // InitAccessModel(model);
             
                model.Country = _interface.GetCountryList();
                model.State = _interface.GetStateList(model.entity.CountryID);
                model.City = _interface.GetCityList(model.entity.StateID);
                model.EntityType = _interface.GetEntityTypeList();
                model.Manager = _interface.GetManagerList();
                return View("Index", model);
            }
            else
            {
                resp.Message = "";
                return Json(resp);
            }
        }

        public IActionResult GetCityList(long StateID)
        { // city dropdown
            return Json(_interface.GetCityList(StateID));
        }

        public IActionResult GetStateList(long CountryID)
        {   // state dropdown
            return Json(_interface.GetStateList(CountryID));
        }

        public IActionResult ExportData(int? exportType)
        {
           
            //  var list = HttpContext.Session.GetObject<IEnumerable<Dipository>>("ExportData").ToList();




            var list = _interface.ListAll().ToList();
            var columnHeader = new string[17] {  "EntityType", "EntityCode", "EntityName", "AddressLine1", "AddressLine2", "Pincode", "Country", "State", "City", "ContactPersonName", "ContactNumber", "ContactEmail", "ManagerName", "IsHO", "SEBIRegistrationNo", "Status", "LastUpdatedDate" };

            //  Export CSV
            if (exportType == 1)
            {

                var browsercsv = new StringBuilder();
                string data = string.Empty;
                int count = list.Count();
                if (list.Count > 0)
                {


                    list.ForEach(line =>
                    {
                        data = list.Count.ToString();
                        data += "," + line.EntityTypeName;
                        data += "," + line.EntityCode;
                        data += "," + line.FirstName + " " + line.LastName;
                        data += "," + line.AddressLine1;
                        data += "," + line.AddressLine2;
                        data += "," + line.Pincode;
                        data += "," + line.CountryName;
                        data += "," + line.StateName;
                        data += "," + line.CityName;
                        data += "," + line.ContactPersonName;
                        data += "," + line.ContactNumber;
                        data += "," + line.ContactEmail;
                        data += "," + line.ManagerName;
                        data += "," + line.IsHO;
                        data += "," + line.SebiRegistration;
                        data += "," + line.IsActiveText;
                        data += "," + line.LastUpdatedDate;

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
                        var workSheet = package.Workbook.Worksheets.Add("Entity Master Data");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {

                            workSheet.Cells[row, 1].Value = list[i].EntityTypeName;
                            workSheet.Cells[row, 2].Value = list[i].EntityCode;
                            workSheet.Cells[row, 3].Value = list[i].FirstName + " " + list[i].LastName;
                            workSheet.Cells[row, 4].Value = list[i].AddressLine1;
                            workSheet.Cells[row, 5].Value = list[i].AddressLine2;
                            workSheet.Cells[row, 6].Value = list[i].Pincode;
                            workSheet.Cells[row, 7].Value = list[i].CountryName;
                            workSheet.Cells[row, 8].Value = list[i].StateName;
                            workSheet.Cells[row, 9].Value = list[i].CityName;
                            workSheet.Cells[row, 10].Value = list[i].ContactPersonName;
                            workSheet.Cells[row, 11].Value = list[i].ContactNumber;
                            workSheet.Cells[row, 12].Value = list[i].ContactEmail;
                            workSheet.Cells[row, 13].Value = list[i].ManagerName;
                            workSheet.Cells[row, 14].Value = list[i].IsHO;
                            workSheet.Cells[row, 15].Value = list[i].SebiRegistration;
                            workSheet.Cells[row, 16].Value = list[i].IsActiveText;
                            workSheet.Cells[row, 17].Value = list[i].LastUpdatedDate;
                            i = i + 1;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Employee Master Data");
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




    }
}
