using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;
using OfficeOpenXml;
using System.Linq;

namespace ZOI.APP.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _interface;

        JsonResponse resp = new JsonResponse();


        [Obsolete]
        public RoleController(IRoleService _IRoleService,  IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IRoleService;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            // returns Role add view

            RoleViewModel model = new RoleViewModel();
            InitAccessModel(model);
            model.role.RoleID = 0;
            model.RoleList = _interface.GetRoleList();
            model.ApplicationList = _interface.GetApplicationList();
            return View(model);

        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(int ID)
        {
            // returns Update view
            RoleViewModel model = new RoleViewModel
            {
                role = _interface.GetData(ID)
            };
            InitAccessModel(model);           
            model.RoleList = _interface.GetRoleList();
            model.ApplicationList = _interface.GetApplicationList();
            if (model.role != null)
            {
                return View("Index", model);
            }
            else
            {
                return Json(resp);
            }
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult IsExstis(string Name)
        { 
            // Check uniqueness of Role Name
            if (!_interface.IsExsits(Name))
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }

        //   [HttpPost]
        //   [IgnoreAntiforgeryToken]
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(RoleViewModel model)
        {  
            // Inserts or Udates Data
            model.role.ID = model.role.Name;
            
            if (ModelState.IsValid)
           {
                return Json(_interface.AddUpdate(model.role));
            }
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);             
            }
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetAll()
        { 
            // gets Role listing data
            return Json(_interface.Summary());
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(int ID, bool Status)
        {  // changes active deacivate status
            return Json(_interface.Deactivate(ID, Status));
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {   // returns listing view
            RoleViewModel model = new RoleViewModel();
            InitAccessModel(model);
            return View(model);
        }

        public IActionResult ExportData(int? exportType)
        {
           // exportType = 2;
            //  var list = HttpContext.Session.GetObject<IEnumerable<Dipository>>("ExportData").ToList();




            var list = _interface.ListAll().ToList();
            var columnHeader = new string[5] { "RoleName", "RoleDescription", "ParentRoleName", "Status", "LastUpdatedDate" };

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
                        data += "," + line.Name;
                        data += "," + line.NormalizedName;
                        data += "," + line.ParentRoleName;
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
                        var workSheet = package.Workbook.Worksheets.Add("Role Master Data");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {
                            workSheet.Cells[row, 1].Value = list[i].Name;
                            workSheet.Cells[row, 2].Value = list[i].NormalizedName;
                            workSheet.Cells[row, 3].Value = list[i].ParentRoleName;
                            workSheet.Cells[row, 4].Value = list[i].IsActiveText;
                            workSheet.Cells[row, 5].Value = list[i].LastUpdatedDate;
                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Role Master Data");
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
