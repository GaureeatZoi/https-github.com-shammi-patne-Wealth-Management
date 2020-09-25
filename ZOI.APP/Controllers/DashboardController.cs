using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using RestSharp;
using ZOI.APP.Models;
using ZOI.BAL.Models;
using ComponentSpace.Saml2;
using Microsoft.AspNetCore.Identity;
using ZOI.BAL;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISamlServiceProvider _samlServiceProvider;
        private readonly IServiceFactory _IServiceFactory;
        public DashboardController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHostingEnvironment AppEnvironment, ISamlServiceProvider samlServiceProvider, IConfiguration configuration, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            _appEnvironment = AppEnvironment;
            _samlServiceProvider = samlServiceProvider;
            _configuration = configuration;
        }
        //public async Task<IActionResult> Index(string returnUrl = null)
        //{
        //    var partnerName = _configuration["PartnerName"];

        //    // To login automatically at the service provider, 
        //    // initiate single sign-on to the identity provider (SP-initiated SSO).            
        //    // The return URL is remembered as SAML relay state.
        //    await _samlServiceProvider.InitiateSsoAsync(partnerName, returnUrl);

        //    return new EmptyResult();
        //}
        public IActionResult Dashboard()
        {
            BaseViewModel model = new BaseViewModel();
            InitAccessModel(model);
            var claims = HttpContext.User.Identity.Name;           
            return View(model);
        }

        public IActionResult ExtendableGrid()
        {
            return View();
        }

        public IActionResult MFWiseReport()
        {
            return View();
        }

        public PartialViewResult GetFilters()
        {
            DashboardFilters model = new DashboardFilters();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetFilterDropdowns");
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            model = JsonConvert.DeserializeObject<DashboardFilters>(response.Content);
            return PartialView("_Filters", model);
        }

        public PartialViewResult GetSliderFilter(string UserID)
        {
            SliderFilter model = new SliderFilter();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetSliderFilter?UserID="+ UserID);
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            model = JsonConvert.DeserializeObject<SliderFilter>(response.Content);
            return PartialView("_SliderFilter",model);
        }

        public PartialViewResult GetCharts(DashboardParameters parameters)
        {
            ProductAssetCharts model = new ProductAssetCharts();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetChartData");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(parameters), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            model = JsonConvert.DeserializeObject<ProductAssetCharts>(response.Content);
            return PartialView("_ProductAssetDetails", model);
        }

        /// <summary>
        /// Method for getting Software mapping list
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>'Session has not been configured for this application or request.'

        public JsonResult GetAssetSummary(DashboardParameters param)
        {
            JsonResponse jsonResponse = new JsonResponse();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetAssetDataSummary");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(response.Content);
            IEnumerable<AssetData> summary = JsonConvert.DeserializeObject <IEnumerable<AssetData>>(jsonResponse.Data.ToString());
            HttpContext.Session.SetObject("GridData", summary);

            int totalconsumable = summary.Count();
            var sortDirection = HttpContext.Request.Query["sSortDir_0"]; // asc or desc
            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.Query["iSortCol_0"]);
            if (!string.IsNullOrEmpty(param.sSearch)) summary = summary.Where(z => z.AssetClass.ToLower().Contains(param.sSearch.ToLower()));
            summary = sortColumnIndex switch
            {
                0 => sortDirection == "asc" ? summary.OrderBy(z => z.AssetClass) : summary.OrderByDescending(z => z.AssetClass),
                1 => sortDirection == "asc" ? summary.OrderBy(z => z.ValueAtCost) : summary.OrderByDescending(z => z.ValueAtCost),
                2 => sortDirection == "asc" ? summary.OrderBy(z => z.MarketValue) : summary.OrderByDescending(z => z.MarketValue),
                3 => sortDirection == "asc" ? summary.OrderBy(z => z.Weightage) : summary.OrderByDescending(z => z.Weightage),
                _ => sortDirection == "desc" ? summary.OrderBy(z => z.UnrealizedGL) : summary.OrderByDescending(z => z.UnrealizedGL),
            };
            int filteredconsumableCount = summary.Count();
            if (param.iDisplayLength > 0)
            {
                summary = summary.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            }
            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalconsumable,
                iTotalDisplayRecords = filteredconsumableCount,
                aaData = summary
            });
        }

        public JsonResult GetCashFlowSummary(DashboardParameters param)
        {
            JsonResponse jsonResponse = new JsonResponse();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetCashFlowDataSummary");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(response.Content);
            IEnumerable<CashFlowData> summary = JsonConvert.DeserializeObject<IEnumerable<CashFlowData>>(jsonResponse.Data.ToString());
            HttpContext.Session.SetObject("CashGridData", summary);
            int totalconsumable = summary.Count();
            var sortDirection = HttpContext.Request.Query["sSortDir_0"]; // asc or desc
            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.Query["iSortCol_0"]);
            if (!string.IsNullOrEmpty(param.sSearch)) summary = summary.Where(z => z.CashFlow.ToLower().Contains(param.sSearch.ToLower()));
            summary = sortColumnIndex switch
            {
                0 => sortDirection == "asc" ? summary.OrderBy(z => z.CashFlow) : summary.OrderByDescending(z => z.CashFlow),
                1 => sortDirection == "asc" ? summary: summary,
                _ => sortDirection == "desc" ? summary : summary,
            };
            int filteredconsumableCount = summary.Count();
            if (param.iDisplayLength > 0)
            {
                summary = summary.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            }
            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalconsumable,
                iTotalDisplayRecords = filteredconsumableCount,
                aaData = summary
            });
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public PartialViewResult GetSlidersData(DashboardParameters parameters)
        {
            Slider model = new Slider();
            var client = new RestClient(CommonFunctions.GetAPIPath() +"Dashboard/GetSliderDetails");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(parameters), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            model.Details = JsonConvert.DeserializeObject<List<Slider>>(response.Content); 
            return PartialView("_SliderDetails",model);
        }

        public JsonResult GetMFSummary(MFWiseSummaryParameters param)
        {
            JsonResponse jsonResponse = new JsonResponse();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetMFSummary");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(response.Content);
            IEnumerable<MFReport> summary = JsonConvert.DeserializeObject<IEnumerable<MFReport>>(jsonResponse.Data.ToString());
            HttpContext.Session.SetObject("GridData", summary);
            int totalconsumable = summary.Count();
            var sortDirection = HttpContext.Request.Query["sSortDir_0"]; // asc or desc
            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.Query["iSortCol_0"]);
            if (!string.IsNullOrEmpty(param.sSearch)) summary = summary.Where(z => z.Scheme.ToLower().Contains(param.sSearch.ToLower()));
            summary = sortColumnIndex switch
            {
                0 => sortDirection == "asc" ? summary.OrderBy(z => z.AMC) : summary,
                1 => sortDirection == "asc" ? summary.OrderBy(z => z.Scheme) : summary.OrderByDescending(z => z.Scheme),
                2 => sortDirection == "asc" ? summary.OrderBy(z => z.Units) : summary.OrderByDescending(z => z.Units),
                3 => sortDirection == "asc" ? summary.OrderBy(z => z.MaturityDate) : summary.OrderByDescending(z => z.MaturityDate),
                4 => sortDirection == "asc" ? summary.OrderBy(z => z.PurchaseNav) : summary.OrderByDescending(z => z.PurchaseNav),
                5 => sortDirection == "asc" ? summary.OrderBy(z => z.ValueAtcost) : summary.OrderByDescending(z => z.ValueAtcost),
                6 => sortDirection == "asc" ? summary.OrderBy(z => z.CurrentNAV) : summary.OrderByDescending(z => z.CurrentNAV),
                7 => sortDirection == "asc" ? summary.OrderBy(z => z.CurrentMarketValuation) : summary.OrderByDescending(z => z.CurrentMarketValuation),
                8 => sortDirection == "asc" ? summary.OrderBy(z => z.Dividend) : summary.OrderByDescending(z => z.Dividend),
                9 => sortDirection == "asc" ? summary.OrderBy(z => z.AbsoluteGainOrLoss) : summary.OrderByDescending(z => z.AbsoluteGainOrLoss),
                10 => sortDirection == "asc" ? summary.OrderBy(z => z.AbsoluteGainORLossPercentage) : summary.OrderByDescending(z => z.AbsoluteGainORLossPercentage),
                11 => sortDirection == "asc" ? summary.OrderBy(z => z.XIRR) : summary.OrderByDescending(z => z.XIRR),
                12 => sortDirection == "asc" ? summary.OrderBy(z => z.AvgDaysInvested) : summary.OrderByDescending(z => z.AvgDaysInvested),
                _ => sortDirection == "desc" ? summary.OrderBy(z => z.PurchaseNav) : summary.OrderByDescending(z => z.PurchaseNav),
            };
            int filteredconsumableCount = summary.Count();
            if (param.iDisplayLength > 0)
            {
                summary = summary.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            }
            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalconsumable,
                iTotalDisplayRecords = filteredconsumableCount,
                aaData = summary
            });
        }

        public JsonResult GetMFChildSummary(AjaxDataTable param)
        {
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetMFChildTableSummary");
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            IEnumerable<MFReport> summary = JsonConvert.DeserializeObject<List<MFReport>>(response.Content);
            HttpContext.Session.SetObject("ChildGridData", summary);
            int totalconsumable = summary.Count();
            var sortDirection = HttpContext.Request.Query["sSortDir_0"]; // asc or desc
            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.Query["iSortCol_0"]);
            if (!string.IsNullOrEmpty(param.sSearch)) summary = summary.Where(z => z.Scheme.ToLower().Contains(param.sSearch.ToLower()));
            summary = sortColumnIndex switch
            {
                1 => sortDirection == "asc" ? summary.OrderBy(z => z.Scheme) : summary.OrderByDescending(z => z.Scheme),
                2 => sortDirection == "asc" ? summary.OrderBy(z => z.Units) : summary.OrderByDescending(z => z.Units),
                3 => sortDirection == "asc" ? summary.OrderBy(z => z.MaturityDate) : summary.OrderByDescending(z => z.MaturityDate),
                4 => sortDirection == "asc" ? summary.OrderBy(z => z.PurchaseNav) : summary.OrderByDescending(z => z.PurchaseNav),
                5 => sortDirection == "asc" ? summary.OrderBy(z => z.ValueAtcost) : summary.OrderByDescending(z => z.ValueAtcost),
                6 => sortDirection == "asc" ? summary.OrderBy(z => z.CurrentNAV) : summary.OrderByDescending(z => z.CurrentNAV),
                7 => sortDirection == "asc" ? summary.OrderBy(z => z.CurrentMarketValuation) : summary.OrderByDescending(z => z.CurrentMarketValuation),
                8 => sortDirection == "asc" ? summary.OrderBy(z => z.Dividend) : summary.OrderByDescending(z => z.Dividend),
                9 => sortDirection == "asc" ? summary.OrderBy(z => z.AbsoluteGainOrLoss) : summary.OrderByDescending(z => z.AbsoluteGainOrLoss),
                10 => sortDirection == "asc" ? summary.OrderBy(z => z.AbsoluteGainORLossPercentage) : summary.OrderByDescending(z => z.AbsoluteGainORLossPercentage),
                11 => sortDirection == "asc" ? summary.OrderBy(z => z.XIRR) : summary.OrderByDescending(z => z.XIRR),
                _ => sortDirection == "desc" ? summary.OrderBy(z => z.PurchaseNav) : summary.OrderByDescending(z => z.PurchaseNav),
            };
            int filteredconsumableCount = summary.Count();
            if (param.iDisplayLength > 0)
            {
                summary = summary.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            }
            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalconsumable,
                iTotalDisplayRecords = filteredconsumableCount,
                aaData = summary
            });
        }

        public async Task<PartialViewResult> GetChildTable(MFWiseSummaryParameters param)
        {
            MFReport model = new MFReport();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetMFChildTableSummary");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            model.MFList = JsonConvert.DeserializeObject<List<MFReport>>(response.Content);
            HttpContext.Session.SetObject("ChildGridData", model.MFList);
            return PartialView("_ChildTable", model);
        }

        public IActionResult ExportData(int? exportType)
        {
            exportType = 2;
            var list = HttpContext.Session.GetObject<List<AssetData>>("GridData");
            // Export CSV
            if (exportType == 1)
            {
                var columnHeader = new string[0];
                var browsercsv = new StringBuilder();
                string data = string.Empty;
                if (list.Count > 0)
                {
                    columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    list.ForEach(line =>
                    {
                        data = list.Count.ToString();
                        data += "," + line.AssetClass;
                        data += "," + line.ValueAtCost;
                        data += "," + line.MarketValue;
                        data += "," + line.Weightage;
                        data += "," + line.UnrealizedGL;
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
                var columnHeader = new String[0];
                if (list.Count > 0)
                {
                    columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    ExcelPackage.LicenseContext= LicenseContext.Commercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Asset Data");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {
                            workSheet.Cells[row, 1].Value = list[i].AssetClass;
                            workSheet.Cells[row, 2].Value = list[i].ValueAtCost;
                            workSheet.Cells[row, 3].Value = list[i].MarketValue;
                            workSheet.Cells[row, 4].Value = list[i].Weightage;
                            workSheet.Cells[row, 5].Value = list[i].UnrealizedGL;
                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Asset Data");
                        workSheet.Cells[1, 1].Value = "No Data Available";
                        package.Save();
                    }
                }
                stream.Position = 0;
                string excelName = $"Asset-data-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";



                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult CashFlowExportData(int? exportType)
        {
            exportType = 2;
            var list = HttpContext.Session.GetObject<List<CashFlowData>>("CashGridData");
            // Export CSV
            if (exportType == 1)
            {
                var columnHeader = new string[0];
                var browsercsv = new StringBuilder();
                string data = string.Empty;
                if (list.Count > 0)
                {
                    columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    list.ForEach(line =>
                    {
                        data = list.Count.ToString();
                        data += "," + line.CashFlow;
                        data += "," + line.AmountInCrore;
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
                var columnHeader = new String[0];
                if (list.Count > 0)
                {
                    columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Cash Flow Data");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {
                            workSheet.Cells[row, 1].Value = list[i].CashFlow;
                            workSheet.Cells[row, 2].Value = list[i].AmountInCrore;
                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Cash Flow Data");
                        workSheet.Cells[1, 1].Value = "No Data Available";
                        package.Save();
                    }
                }
                stream.Position = 0;
                string excelName = $"cash-flow-data-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";



                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult MFExportData(int? exportType)
        {
            exportType = 2;
            var list = HttpContext.Session.GetObject<List<MFReport>>("GridData");
            // Export CSV
            if (exportType == 1)
            {
                var columnHeader = new string[0];
                var browsercsv = new StringBuilder();
                string data = string.Empty;
                if (list.Count > 0)
                {
                    columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    list.ForEach(line =>
                    {
                        data = list.Count.ToString();
                        data += "," + line.AMC;
                        data += "," + line.Scheme;
                        data += "," + line.MaturityDate;
                        data += "," + line.Units;
                        data += "," + line.PurchaseNav;
                        data += "," + line.ValueAtcost;
                        data += "," + line.CurrentNAV;
                        data += "," + line.CurrentMarketValuation;
                        data += "," + line.Dividend;
                        data += "," + line.AbsoluteGainOrLoss;
                        data += "," + line.AbsoluteGainORLossPercentage;
                        data += "," + line.XIRR;
                        data += "," + line.AvgDaysInvested;
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
                var columnHeader = new String[0];
                if (list.Count > 0)
                {
                    columnHeader = GetHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("MFReport");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {
                            workSheet.Cells[row, 1].Value = list[i].ID;
                            workSheet.Cells[row, 2].Value = list[i].AMC;
                            workSheet.Cells[row, 3].Value = list[i].Scheme;
                            workSheet.Cells[row, 4].Value = list[i].MaturityDate;
                            workSheet.Cells[row, 5].Value = list[i].Units;
                            workSheet.Cells[row, 6].Value = list[i].PurchaseNav;
                            workSheet.Cells[row, 7].Value = list[i].ValueAtcost;
                            workSheet.Cells[row, 8].Value = list[i].CurrentNAV;
                            workSheet.Cells[row, 9].Value = list[i].CurrentMarketValuation;
                            workSheet.Cells[row, 10].Value = list[i].Dividend;
                            workSheet.Cells[row, 11].Value = list[i].AbsoluteGainOrLoss;
                            workSheet.Cells[row, 12].Value = list[i].AbsoluteGainORLossPercentage;
                            workSheet.Cells[row, 13].Value = list[i].XIRR;
                            workSheet.Cells[row, 14].Value = list[i].AvgDaysInvested;
                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("MFReport");
                        workSheet.Cells[1, 1].Value = "No Data Available";
                        package.Save();
                    }
                }
                stream.Position = 0;
                string excelName = $"MF-Wise-Summary-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";



                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return NotFound();
            }
        }

        public string[] GetHeaderValues(object modelClass)
        {
            PropertyInfo[] listPI = modelClass.GetType().GetProperties();
            List<string> headerValues = new List<string>();
            string displayName = string.Empty;
            foreach (PropertyInfo pi in listPI)
            {
                if (pi != null && pi.Name != "TotalCount" && pi.Name != "MFList" && pi.Name != "LastUpdatedOn" && pi.Name != "SortDate" && pi.Name != "AssetList" && pi.Name != "cashFlowDatasList")
                {
                    displayName = pi.Name;
                    headerValues.Add(displayName);
                }
            }
            return headerValues.ToArray();
        }

        public string[] GetChildTableHeaderValues(object modelClass)
        {
            PropertyInfo[] listPI = modelClass.GetType().GetProperties();
            List<string> headerValues = new List<string>();
            string displayName = string.Empty;
            foreach (PropertyInfo pi in listPI)
            {
                if (pi != null && pi.Name != "TotalCount" && pi.Name != "MFList" && pi.Name != "LastUpdatedOn" && pi.Name != "SortDate" && pi.Name != "ID" && pi.Name != "AMC" && pi.Name != "AvgDaysInvested")
                {
                    displayName = pi.Name;
                    headerValues.Add(displayName);
                }
            }
            return headerValues.ToArray();
        }
        public async Task<IActionResult> OnPostExport()
        {
            string sWebRootFolder = _appEnvironment.WebRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(sFileName);
                IRow row = excelSheet.CreateRow(0);
                var obj =HttpContext.Session.GetObject<List<AssetData>>("GridData");
                row.CreateCell(0).SetCellValue("ID");
                row.CreateCell(1).SetCellValue("Name");
                row.CreateCell(2).SetCellValue("Age");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue(1);
                row.CreateCell(1).SetCellValue("Kane Williamson");
                row.CreateCell(2).SetCellValue(29);

                row = excelSheet.CreateRow(2);
                row.CreateCell(0).SetCellValue(2);
                row.CreateCell(1).SetCellValue("Martin Guptil");
                row.CreateCell(2).SetCellValue(33);

                row = excelSheet.CreateRow(3);
                row.CreateCell(0).SetCellValue(3);
                row.CreateCell(1).SetCellValue("Colin Munro");
                row.CreateCell(2).SetCellValue(23);

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public PartialViewResult GetMFSummaryFilters()
        {
            MFSummaryFilter model = new MFSummaryFilter();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetMFSummaryFilters");
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            model = JsonConvert.DeserializeObject<MFSummaryFilter>(response.Content);
            return PartialView("_MFSummaryFilters", model);
        }

        public IActionResult ChildTableExportData(int? exportType)
        {
            exportType = 2;
            var list = HttpContext.Session.GetObject<List<MFReport>>("ChildGridData");
            // Export CSV
            if (exportType == 1)
            {
                var columnHeader = new string[0];
                var browsercsv = new StringBuilder();
                string data = string.Empty;
                if (list.Count > 0)
                {
                    columnHeader = GetChildTableHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    list.ForEach(line =>
                    {
                        data = list.Count.ToString();
                        data += "," + line.Scheme;
                        data += "," + line.MaturityDate;
                        data += "," + line.Units;
                        data += "," + line.PurchaseNav;
                        data += "," + line.ValueAtcost;
                        data += "," + line.CurrentNAV;
                        data += "," + line.CurrentMarketValuation;
                        data += "," + line.Dividend;
                        data += "," + line.AbsoluteGainOrLoss;
                        data += "," + line.AbsoluteGainORLossPercentage;
                        data += "," + line.XIRR;
                        data += "," + line.AvgDaysInvested;
                        browsercsv.AppendLine(data);
                    });
                }
                else
                {
                    data = "No Data Available";
                    browsercsv.AppendLine(data);
                }
                string csvName = $"MF-Wise-Transactions-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.csv";
                byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeader)}\r\n{browsercsv.ToString()}");
                return File(buffer, "text/csv", csvName);
            }
            //Export Excel
            else if (exportType == 2)
            {
                var stream = new MemoryStream();
                var columnHeader = new String[0];
                if (list.Count > 0)
                {
                    columnHeader = GetChildTableHeaderValues(list.First());// appSumaryModel.data.First().GetType().GetProperties().Where(i => i.CanRead).Select(i => i.Name).ToArray();
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("MFReport");
                        int totalRows = list.Count;
                        for (var j = 0; j < columnHeader.Length; j++)
                        {
                            workSheet.Cells[1, j + 1].Value = columnHeader[j];
                            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
                        }
                        int i = 0;
                        for (int row = 2; row <= totalRows + 1; row++)
                        {
                            workSheet.Cells[row, 1].Value = list[i].Scheme;
                            workSheet.Cells[row, 2].Value = list[i].MaturityDate;
                            workSheet.Cells[row, 3].Value = list[i].Units;
                            workSheet.Cells[row, 4].Value = list[i].PurchaseNav;
                            workSheet.Cells[row, 5].Value = list[i].ValueAtcost;
                            workSheet.Cells[row, 6].Value = list[i].CurrentNAV;
                            workSheet.Cells[row, 7].Value = list[i].CurrentMarketValuation;
                            workSheet.Cells[row, 8].Value = list[i].Dividend;
                            workSheet.Cells[row, 9].Value = list[i].AbsoluteGainOrLoss;
                            workSheet.Cells[row, 10].Value = list[i].AbsoluteGainORLossPercentage;
                            workSheet.Cells[row, 11].Value = list[i].XIRR;
                            workSheet.Cells[row, 12].Value = list[i].AvgDaysInvested;
                            i++;
                        }
                        package.Save();
                    }
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("MFReport");
                        workSheet.Cells[1, 1].Value = "No Data Available";
                        package.Save();
                    }
                }
                stream.Position = 0;
                string excelName = $"MF-Wise-Transactions-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<PartialViewResult> GetAssetTable(DashboardParameters param)
        {
            AssetData model = new AssetData();
            JsonResponse jsonResponse = new JsonResponse();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetAssetDataSummary");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(response.Content);
            model.AssetList = JsonConvert.DeserializeObject<List<AssetData>>(jsonResponse.Data.ToString());
            model.LastUpdatedOn = jsonResponse.LastUpdatedOn;
            HttpContext.Session.SetObject("GridData", model.AssetList);
            return PartialView("_AssetDataTable", model);
        }

        public async Task<PartialViewResult> GetCashFlowTable(DashboardParameters param)
        {
            CashFlowData model = new CashFlowData();
            JsonResponse jsonResponse = new JsonResponse();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetCashFlowDataSummary");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(param), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(response.Content);
            model.cashFlowDatasList = JsonConvert.DeserializeObject<List<CashFlowData>>(jsonResponse.Data.ToString());
            model.LastUpdatedOn = jsonResponse.LastUpdatedOn;
            HttpContext.Session.SetObject("CashGridData", model.cashFlowDatasList);
            return PartialView("_CashFlowDataTable", model);
        }

        public JsonResult GetCustomer(string userID)
        {
            JsonResponse jsonResponse = new JsonResponse();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetCustomer?userID="+ userID);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            GetGeneralFilters getGeneralFilters = new GetGeneralFilters();
            getGeneralFilters = JsonConvert.DeserializeObject<GetGeneralFilters>(response.Content);
            jsonResponse.Data = getGeneralFilters.data;
            jsonResponse.InceptionDate = getGeneralFilters.InceptionDate;
            return Json(jsonResponse);
        }

        public JsonResult GetportfolioType(int CustomerID, string userID)
        {
            JsonResponse jsonResponse = new JsonResponse();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetportfolioType?CustomerID=" + CustomerID + "&userID="+ userID);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            GetGeneralFilters getGeneralFilters = new GetGeneralFilters();
            getGeneralFilters = JsonConvert.DeserializeObject<GetGeneralFilters>(response.Content);
            jsonResponse.Data = getGeneralFilters.data;
            jsonResponse.InceptionDate = getGeneralFilters.InceptionDate;
            return Json(jsonResponse);
        }

        public JsonResult GetAccount(int PortfolioTypeID, string userID, int CustomerID)
        {
            JsonResponse jsonResponse = new JsonResponse();
            var client = new RestClient(CommonFunctions.GetAPIPath() + "Dashboard/GetAccount?PortfolioTypeID=" + PortfolioTypeID + "&userID=" + userID + "&CustomerID=" + CustomerID);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            GetGeneralFilters getGeneralFilters = new GetGeneralFilters();
            getGeneralFilters  = JsonConvert.DeserializeObject<GetGeneralFilters>(response.Content);
            jsonResponse.Data = getGeneralFilters.data;
            jsonResponse.InceptionDate = getGeneralFilters.InceptionDate;
            return Json(jsonResponse);
        }

    }
}