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
    public class StateController : BaseController
    {   private readonly IStateService _StateService;


        [Obsolete]
        private readonly IHostingEnvironment _HostingEnvironment;

        [Obsolete]

        public StateController(IStateService stateService, IHostingEnvironment HostingEnvironment, IServiceFactory serviceFactory) : base(serviceFactory)
        { _StateService = stateService;
            _HostingEnvironment = HostingEnvironment;
        }

        public State State { get; set; }
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            StateViewModel model = new StateViewModel();
            InitAccessModel(model);
            return View(model);

        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetAll()
        { // gets  listing data


            return Json(_StateService.Summary());
        }


        [HttpGet]
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IEnumerable<State> GetAllStates()
        {  //Return Listing of State Data
            IEnumerable<State> StateData;
            StateData = _StateService.ListAll();
            HttpContext.Session.SetObject("ExportData", StateData);
          
            return StateData;
        }

        [HttpGet]
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index(long id)
        {   // Return View of Add Update Page

           
            StateViewModel model = new StateViewModel();
            model.CountryList = _StateService.InitStateView();
            if (id != 0)
            {
               

                model.state = _StateService.Find(id);
                model.CountryList = _StateService.InitStateView();
            }
            if (model.state == null)
            {   return NotFound();
            }
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(StateViewModel model)
        {  // Insert Update State
            JsonResponse resp = new JsonResponse();
            if (ModelState.IsValid)
            {  return Json(_StateService.AddUpdate(model.state));
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
            return Json(_StateService.Deactivate(ID, Status));
        }



        
        //----------------------------------------------------------------------

        public IActionResult ExportData(int? exportType)
        {
            exportType = 2;
            // var list = HttpContext.Session.GetObject<IEnumerable<State>>("ExportData").ToList();

            var list = _StateService.ListAll().ToList();

            // Export CSV
            if (exportType == 1)
            {
                var columnHeader = new string[0];
                var browsercsv = new StringBuilder();
                string data = string.Empty;
             //   int count = list.Count();
                if (list.Count > 0)
                {
                   // columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    list.ForEach(line =>
                    {
                        data = list.Count.ToString();
                        data += "," + line.StateName;
                        data += "," + line.CountryName;
                        data += "," + line.StateCode;
                        data += "," + line.GSTStateCode;
                        data += "," + line.IsUnionTerritory;
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
           
                var columnHeader = new string[6] {"StateName","Country Name","State Code","GST State Code","Is Union Territory","IsActive" };
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
                            workSheet.Cells[row, 1].Value = list[i].StateName;
                            workSheet.Cells[row, 2].Value = list[i].CountryName;
                            workSheet.Cells[row, 3].Value = list[i].StateCode;
                            workSheet.Cells[row, 4].Value = list[i].GSTStateCode;
                            workSheet.Cells[row, 5].Value = list[i].IsUnionTerritory;
                            workSheet.Cells[row, 6].Value = list[i].IsActive;
                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("State Master Data");
                        workSheet.Cells[1, 1].Value = "No Data Available";
                        package.Save();
                    }
                }
                stream.Position = 0;
                string excelName = $"StateMasterData-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";



                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return NotFound();
            }
        }

        //------------------------------------------------------------------------
        public IActionResult IsStateExists(string name)
        {  // Check Uniqueness  of StateName
            JsonResponse resp = new JsonResponse();
            if (!_StateService.IsStateExists(name))
            {
                 resp.Status = Constants.ResponseStatus.Success;
                resp.Message = "";
            }
            else
            {
                resp.Status = "F";
                 resp.Message = Constants.ControllerMessage.Data_Exsists;

            }
            return Json(resp);
        }


        public IActionResult IsgstcodeExists(int name)
        {  // Check Uniqueness  of gstcode
            JsonResponse resp = new JsonResponse();
            if (!_StateService.IsgstcodeExists(name))
            {
                resp.Status = Constants.ResponseStatus.Success;
                resp.Message = "";
            }
            else
            {
                resp.Status = "F";
                resp.Message = Constants.ControllerMessage.Data_Exsists;

            }
            return Json(resp);
        }
    }
}
