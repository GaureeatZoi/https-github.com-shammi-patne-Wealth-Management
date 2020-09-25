using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;
using OfficeOpenXml;
using System.Linq;

namespace ZOI.APP.Controllers
{
    public class EmployeeController : BaseController
    {

        private readonly IEmployeeService _interface;

        JsonResponse resp = new JsonResponse();
      
        public EmployeeController(IEmployeeService _IEmployeeService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IEmployeeService;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {

            EmployeeViewModel model = new EmployeeViewModel();
            InitAccessModel(model);
            model.DepartmentList = _interface.GetDepartmentList();
            model.SubDepartmentList = _interface.GetSubDepartmentList(0);
            model.DesignationList = _interface.GetDesignationList();
            model.Gender = _interface.GetGenderList();
            model.ReportingTo = _interface.GetReportingToList();
            model.Role = _interface.GetRoleList();
            return View(model);
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(int ID)
        {  // returns Update view

            EmployeeViewModel model = new EmployeeViewModel();
            InitAccessModel(model);
            model.employee = _interface.GetData(ID);
            if (model != null)
            {
                InitAccessModel(model);
                model.DepartmentList = _interface.GetDepartmentList();
                model.SubDepartmentList = _interface.GetSubDepartmentList(model.employee.DepartmentID);
                model.DesignationList = _interface.GetDesignationList();
                model.Gender = _interface.GetGenderList();
                model.ReportingTo = _interface.GetReportingToList();
                model.Role = _interface.GetRoleList();
                return View("Index", model);
            }
            else
            {
                return Json(resp);
            }
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(EmployeeViewModel model)
        {  // Inserts or Udates Data
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.employee));
            }
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);

            }
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult IsExstis(string Name, int ID)
        { // Check uniqueness of Employee date
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
        { // gets employee listing data


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
            EmployeeViewModel model = new EmployeeViewModel();
            InitAccessModel(model);
            return View(model);
        }



       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult GetSubDeptList(int DeptID)
        { // Sub DEpartmnt dropdown
            return Json(_interface.GetSubDepartmentList(DeptID));
        }

        public IActionResult ExportData(int? exportType)
        {
            //exportType = 2;
            //  var list = HttpContext.Session.GetObject<IEnumerable<Dipository>>("ExportData").ToList();
            var list = _interface.ListAll().ToList();
            var columnHeader = new string[15] { "EmployeeCode", "EmployeeName", "Gender", "Email", "MobileNo", "Department", "SubDepartment", "ReportingTo", "DOB", "PAN", "CertificationNo", "Designation", "Role", "Status", "LastUpdatedDate"  };

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
                        data += "," + line.EmployeeCode;
                        data += "," + line.EmployeeName;
                        data += "," + line.Gender;
                        data += "," + line.Email;
                        data += "," + line.MobileNo;
                        data += "," + line.Department;
                        data += "," + line.SubDepartment;
                        data += "," + line.ReportingName;
                        data += "," + line.DOB;
                        data += "," + line.PAN;
                        data += "," + line.CertificationNo;
                        data += "," + line.Designation;
                        data += "," + line.Role;
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
                        var workSheet = package.Workbook.Worksheets.Add("Employee Master Data");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {

                            workSheet.Cells[row, 1].Value = list[i].EmployeeCode;
                            workSheet.Cells[row, 2].Value = list[i].EmployeeName;
                            workSheet.Cells[row, 3].Value = list[i].Gender;
                            workSheet.Cells[row, 5].Value = list[i].Email;
                            workSheet.Cells[row, 6].Value = list[i].MobileNo;
                            workSheet.Cells[row, 7].Value = list[i].Department;
                            workSheet.Cells[row, 8].Value = list[i].SubDepartment;
                            workSheet.Cells[row, 9].Value = list[i].ReportingName;
                            workSheet.Cells[row, 10].Value = list[i].DOB;
                            workSheet.Cells[row, 11].Value = list[i].PAN;
                            workSheet.Cells[row, 12].Value = list[i].CertificationNo;
                            workSheet.Cells[row, 13].Value = list[i].Designation;
                            workSheet.Cells[row, 14].Value = list[i].Role;
                            workSheet.Cells[row, 15].Value = list[i].IsActiveText;
                            workSheet.Cells[row, 16].Value = list[i].LastUpdatedDate;
                            i++;
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
