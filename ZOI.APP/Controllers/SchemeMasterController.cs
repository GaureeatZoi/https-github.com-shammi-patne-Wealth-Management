using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class SchemeMasterController : BaseController
    {
        private readonly ISchemeService _schemeService;
        public SchemeMasterController(ISchemeService schemeService,IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _schemeService = schemeService;
        }

        #region Add,Update,Listing of SchemeMaster

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            SchemeViewModel model = new SchemeViewModel();
            model.RtaList = _schemeService.GetRTAList();
            model.SchemeTypeList = _schemeService.GetSchemeTypeList();
            model.AmcList = _schemeService.GetAmcList();
            model.PlanList = _schemeService.GetPlanList();
            model.OptionTypeList = _schemeService.GetOPtionTypeList();
            model.RestrictNationalityList = _schemeService.GetCountryList();
            model.InvestorTypeList = _schemeService.GetCountryList();
            model.SeriesList = _schemeService.GetSeriesList();
            model.AssetClassList = _schemeService.GetAssetClassList();
            model.SubAssetClassList= _schemeService.GetAssetClassList();
            model.NoOfDaysList = _schemeService.GetMinimumHoldingPeriodDaysList();
            model.NoOfMonthsList = _schemeService.GetMinimumHoldingPeriodMonthsList();
            model.NoOfYearsList = _schemeService.GetMinimumHoldingPeriodYearsList();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Add Update Scheme Master
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(SchemeViewModel model)
        {
            JsonResponse response = new JsonResponse();
            // If the model valid go to the next level
            if (ModelState.IsValid)
            {
                response = _schemeService.AddUpdate(model);
                return Json(response);
            }// Else it  shows the error message.
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
        /// Get the Data based on ID and return to the model to update view.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns> 
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(int ID)
        {
            SchemeViewModel model = new SchemeViewModel
            {
                schemeMaster = _schemeService.GetData(ID),
                SchemeTransactionDetails = _schemeService.GetTransactionData(ID),
                schemeRegistrationDetails = _schemeService.GetRegistrationData(ID),
                Frequency = _schemeService.GetFrequncyData(ID),
                schemeRestrictedNationality= _schemeService.GetRestrictedNationality(ID),
                schemeRestrictedInvestorType=_schemeService.GetRestrictedInvestorData(ID)
            };
            if (model.schemeMaster != null)
            {
                model.RtaList = _schemeService.GetRTAList();
                model.SchemeTypeList = _schemeService.GetSchemeTypeList();
                model.AmcList = _schemeService.GetAmcList();
                model.PlanList = _schemeService.GetPlanList();
                model.OptionTypeList = _schemeService.GetOPtionTypeList();
                model.RestrictNationalityList = _schemeService.GetCountryList();
                model.InvestorTypeList = _schemeService.GetCountryList();
                model.SeriesList = _schemeService.GetSeriesList();
                model.AssetClassList = _schemeService.GetAssetClassList();
                model.SubAssetClassList = _schemeService.GetAssetClassList();
                model.NoOfDaysList = _schemeService.GetMinimumHoldingPeriodDaysList();
                model.NoOfMonthsList = _schemeService.GetMinimumHoldingPeriodMonthsList();
                model.NoOfYearsList = _schemeService.GetMinimumHoldingPeriodYearsList();
                model.SchemeTransactionDetails.RestrictedNationality = _schemeService.GetSchemesRestrictedNationality(ID).ToArray();
                model.SchemeTransactionDetails.InvestorType = _schemeService.GetSchemesRestrictedInvestorType(ID).ToArray();
                //InitAccessModel(model);
                return View("Index", model);
            }
            else
            {

                return View("SummaryView", model);
            }
        }
        public IActionResult IsExstis(string name, long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_schemeService.IsExsits(name, ID))
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
        public IActionResult SummaryView()
        {
            SchemeViewModel model = new SchemeViewModel();
            InitAccessModel(model);
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_schemeService.Summary());
        }

        /// <summary>
        /// Change the Status of the data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Json response</returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_schemeService.Deactivate(ID, Status));
        }
       
        #endregion
    }
}
