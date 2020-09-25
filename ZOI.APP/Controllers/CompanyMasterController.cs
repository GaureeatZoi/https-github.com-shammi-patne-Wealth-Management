using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class CompanyMasterController : BaseController
    {
        private readonly ICompanyService _companyService;

        [Obsolete]
        private readonly IHostingEnvironment _HostingEnvironment;
        public CompanyMasterController(ICompanyService companyService, IHostingEnvironment HostingEnvironment, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _companyService = companyService;
            _HostingEnvironment = HostingEnvironment;
        }

        #region Add,Update,Listing of Company master

        /// <summary>
        /// Add and update view 
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            CompanyViewModel model = new CompanyViewModel {
                CountryList = _companyService.GetCountryList()
            };
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Company summary
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            CompanyViewModel model = new CompanyViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Get Company code
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCompanyCode()
        {
            long id = _companyService.GetCompanyCode();
            return Json(id);
        }

        /// <summary>
        /// Check duplicate for company name 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IActionResult IsExstis(string name, long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_companyService.IsExsits(name, ID))
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }

        /// <summary>
        /// Get cities for dropdown
        /// </summary>
        /// <param name="StateID"></param>
        /// <returns></returns>
        public IActionResult GetCityList(long StateID)
        {
            return Json(_companyService.GetCityList(StateID));
        }

        /// <summary>
        ///  Get states for dropdown
        /// </summary>
        /// <param name="CountryID"></param>
        /// <returns></returns>
        public IActionResult GetStateList(long CountryID)
        {
            return Json(_companyService.GetStateList(CountryID));
        }

        /// <summary>
        /// Upload Icon
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Obsolete]
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public JsonResult UploadIcon(CompanyMaster modal)
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                if (modal != null && modal.IconFile != null && modal.IconFile.Length > 0)
                {
                    var filename = Path.Combine(modal.IconFile.FileName);
                    var path = Path.Combine(_HostingEnvironment.WebRootPath, "Uploaded_Image", filename);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        modal.IconFile.CopyTo(fileStream);
                    }
                    resp.Data = filename;
                    resp.Status = Constants.ResponseStatus.Success;

                }
                else if (modal.Icon != null && modal.ID != 0)
                {
                    return Json(_companyService.CheckImage(modal));
                }
                else
                {
                    resp.Message = Constants.ControllerMessage.Upload_Needed;
                }
            }
            catch (Exception ex)
            {
                resp.Message = Constants.ControllerMessage.Upload_Needed;
            }
            return Json(resp);
        }

        /// <summary>
        /// Add update Company
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(CompanyViewModel model)
        {
            // If the model valid go to the next level
            if (ModelState.IsValid)
            {
                return Json(_companyService.AddUpdate(model.CompanyMaster));
            }
            // Else it  shows the error message.
            else
            {
                JsonResponse resp = new JsonResponse
                {
                    Message = Constants.ControllerMessage.All_Fields_Mandatory
                };
                return Json(resp);
            }
        }

        /// <summary>
        /// Get company details for Listing
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            var data = _companyService.Summary();
            return Json(data);
        }

        /// <summary>
        /// Update Company Master
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(int ID)
        {
            CompanyViewModel model = new CompanyViewModel
            {
                CompanyMaster = _companyService.GetData(ID)
               
            };
            if (model.CompanyMaster != null)
            {
                model.CompanyMaster.Icon = Encoding.UTF8.GetString(model.CompanyMaster.IconFileData);
                model.CountryList = _companyService.GetCountryList();
                model.StateList = _companyService.GetStateList(model.CompanyMaster.CountryID);
                model.CityList = _companyService.GetCityList(model.CompanyMaster.StateID);
                //var path = Path.Combine(_HostingEnvironment.WebRootPath, "Uploaded_Image", model.CompanyMaster.Icon);
                //using (var fileStream = new FileStream(path, FileMode.Open))
                //{
                //    model.CompanyMaster.IconFile = 
                //}
                InitAccessModel(model);
                return View("Index", model);
            }
            else
            {
                return RedirectToAction("DateNotFound", "Home");
            }

        }

        /// <summary>
        /// Change status of Company records
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_companyService.Deactivate(ID, Status));
        }

        #endregion
    }
}
