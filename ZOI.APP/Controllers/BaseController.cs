using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;
using ZOI.DAL.DatabaseUtility.Services;

namespace ZOI.APP.Controllers
{
    public class BaseController : Controller
    {
        protected IServiceFactory serviceFactory;

        public BaseController(IServiceFactory _serviceFactory)
        {
            serviceFactory = _serviceFactory;
        }
        public void InitAccessModel(BaseViewModel viewModel)
        {            
            int RoleID = Convert.ToInt32(GetUserID("RoleID"));
            //viewModel.Menus = serviceFactory.GetService<BaseService>().FindUserMenus(RoleID);
            viewModel.Menus = serviceFactory.GetService<BaseService>().FindUserMenus(1); 
            string controller = ControllerContext.RouteData.Values["controller"].ToString();
            if (controller != "Home")
            {
                viewModel.CurrentMenuPermission = serviceFactory.GetService<BaseService>().CurrentMenuPermission(controller, RoleID);
            }
            var identity = (ClaimsIdentity)User.Identity;
            viewModel.UserClaims = identity.Claims;
        }

        public string GetUserID(string key)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            return identity.Claims.Where(c => c.Type == key)
                  .Select(c => c.Value).SingleOrDefault();
        }


        //public IActionResult ExportData(int? exportType)
        //{  
        //    // Exports data in csv or excell format            
        //    // var list = HttpContext.Session.GetObject<IEnumerable<City>>("CityMasterData").ToList();

        //    var list = _CityService.ListAll().ToList();

        //    // Export CSV
        //    if (exportType == 1)
        //    {
        //        var columnHeader = new string[0];
        //        var browsercsv = new StringBuilder();
        //        string data = string.Empty;
        //        //   int count = list.Count();
        //        if (list.Count > 0)
        //        {
        //            // columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
        //            list.ForEach(line =>
        //            {
        //                data = list.Count.ToString();
        //                data += "," + line.CityName;
        //                data += "," + line.StateName;

        //                data += "," + line.CountryName;
        //                data += "," + line.IsActive;
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
        //    //Export Excel
        //    else if (exportType == 2)
        //    {
        //        var stream = new MemoryStream();

        //        var columnHeader = GetHeaderValues(list.First());
        //        if (list.Count > 0)
        //        {
        //            // columnHeader = GetHeaderValues(list.First());
        //             // appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
        //            ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //            using (var package = new ExcelPackage(stream))
        //            {
        //                var workSheet = package.Workbook.Worksheets.Add("State Master Data");
        //                int totalRows = list.Count;
        //                for (var j = 0; j < columnHeader.Length; j++)
        //                {
        //                    workSheet.Cells[1, j + 1].Value = columnHeader[j];
        //                    workSheet.Cells[1, j + 1].Style.Font.Bold = true;
        //                }
        //                int i = 0;
        //                for (int row = 2; row <= totalRows + 1; row++)
        //                {
        //                    workSheet.Cells[row, 1].Value = list[i].CityName;
        //                    workSheet.Cells[row, 2].Value = list[i].StateName;
        //                    workSheet.Cells[row, 3].Value = list[i].CountryName;
        //                    workSheet.Cells[row, 4].Value = list[i].IsActive;
        //                    i++;
        //                }
        //                package.Save();
        //            }
        //        }
        //        else
        //        {
        //            using (var package = new ExcelPackage(stream))
        //            {
        //                var workSheet = package.Workbook.Worksheets.Add("City Master Data");
        //                workSheet.Cells[1, 1].Value = "No Data Available";
        //                package.Save();
        //            }
        //        }
        //        stream.Position = 0;
        //        string excelName = $"CityMasterData-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
        //        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //public string[] GetHeaderValues(object modelClass)
        //{
        //    PropertyInfo[] listPI = modelClass.GetType().GetProperties();
        //    List<string> headerValues = new List<string>();
        //    string displayName = string.Empty;
        //    foreach (PropertyInfo pi in listPI)
        //    {
        //        if (pi != null && pi.Name != "TotalCount" && pi.Name != "MFList" && pi.Name != "LastUpdatedOn" && pi.Name != "SortDate" && pi.Name != "AssetList" && pi.Name != "cashFlowDatasList")
        //        {
        //            displayName = pi.Name;
        //            headerValues.Add(displayName);
        //        }
        //    }
        //    return headerValues.ToArray();
        //}

    }
}
