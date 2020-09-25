using System;
using System.IO;
using System.Text;
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
    public class MenuController : BaseController
    {
        private readonly IMenuService _interface;

        JsonResponse resp = new JsonResponse();

        public Menu Menu { get; set; }

        public MenuController(IMenuService _IMenuService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IMenuService;
        }



       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {  // returns Menu add view
            MenuViewModel model = new MenuViewModel();
            model.menu.ID =0;
            model.MenuList = _interface.GetAllMenuList();
            model.GroupList = _interface.GetGroupList();
            InitAccessModel(model);
            return View(model);
        }

                
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(long ID)
        {  // returns Update view

            MenuViewModel model = new MenuViewModel
            {
                menu = _interface.GetData(ID),
                MenuList = _interface.GetAllMenuList(),
                GroupList = _interface.GetGroupList(),
                SubMenuList = _interface.GetSubMenuList(),
            };
            InitAccessModel(model);
            if (model.menu != null)
            {
                return View("Index", model);
            }
            else
            {
                resp.Message = Constants.Service.Common_message;
                return Json(resp);
            }

        }
        
        
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult IsExstis(string Name)
        { // Check uniqueness of Menu Name
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
        
        
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(MenuViewModel model)
        {  // Inserts or Udates Data            

            if (model.menu.IsParentMenu == true)
            {   if (model.menu.MenuName != "" && model.menu.MenuIcon != "" && model.menu.MenuOrder != 0)
                { 
                    return Json(_interface.AddUpdate(model.menu));
                }
                else
                {
                    resp.Status = "F";
                    resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                    return Json(resp);
                }
            }
            else
            {
                if (model.menu.MenuName != "" && model.menu.ParentMenuId != 0 && model.menu.MenuIcon != "" && model.menu.MenuOrder != 0 && model.menu.ActionName != "" && model.menu.ControllerName != "")
                {
                    return Json(_interface.AddUpdate(model.menu));
                }
                else
                {
                    resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                    return Json(resp);
                }
            }

        }

        
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetSummary()   
        { // gets Menu listing data
            return Json(_interface.Summary());
        }

        
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {  // changes active deacivate status
            return Json(_interface.Deactivate(ID, Status));
        } 
        
      
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {   // returns listing view
            MenuViewModel model = new MenuViewModel();
            InitAccessModel(model);
            return View(model);
                     
        }

        public IActionResult ExportData(int? exportType)
        {
            //exportType = 2;
            //  var list = HttpContext.Session.GetObject<IEnumerable<Dipository>>("ExportData").ToList();




            var list = _interface.ListAll().ToList();
            var columnHeader = new string[9] { "MenuName", "ParentMenuName", "ControllerName", "ActionName", "Params1", "Params2", "IsParentMenuText", "Status", "LastUpdatedDate" };

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
                        data += "," + line.MenuName;
                        data += "," + line.ParentMenuName;
                        data += "," + line.ControllerName;
                        data += "," + line.ActionName;
                        data += "," + line.Params1;
                        data += "," + line.Params2;
                        data += "," + line.IsParentMenuText;
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
                        var workSheet = package.Workbook.Worksheets.Add("Menu Master Data");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {
                            workSheet.Cells[row, 1].Value = list[i].MenuName;
                            workSheet.Cells[row, 2].Value = list[i].ParentMenuName;
                            workSheet.Cells[row, 3].Value = list[i].ControllerName;
                            workSheet.Cells[row, 4].Value = list[i].ActionName;
                            workSheet.Cells[row, 5].Value = list[i].Params1;
                            workSheet.Cells[row, 6].Value = list[i].Params2;
                            workSheet.Cells[row, 7].Value = list[i].IsParentMenuText;
                            workSheet.Cells[row, 8].Value = list[i].IsActiveText;
                            workSheet.Cells[row, 9].Value = list[i].LastUpdatedDate;
                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Menu Master Data");
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


        public IActionResult FillMenuDropdown(int Flag)
        {
            if (Flag == 2)
            {
                return Json(_interface.GetAllMenuList());
            }
            else
            {
                return Json(_interface.GetSubMenuList());
            }           
        }


    }
}
