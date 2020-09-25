using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class ClientMainController : BaseController
    {
        private readonly IClientMainService _interface;

        JsonResponse resp = new JsonResponse();

        public ClientMainController(IClientMainService _IClientMainService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IClientMainService;
        }

        /// <summary>
        /// Add and Update view for the ClientMain master
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            ClientMainViewModel model = new ClientMainViewModel();
            model.BankAccountTypeList = _interface.FillEnumList("BankAccountType");
            model.AnnualIncomeList = _interface.FillEnumList("AnnualIncome");
            model.CitizenShipList = _interface.FillEnumList("Citizenship");
            model.MaterialList = _interface.FillEnumList("MaterialStatus");
            model.ResidentalList = _interface.FillEnumList("ResidentialStatus");
            model.TradingExperienceList = _interface.FillEnumList("TradingExperience");
            model.EducationList = _interface.FillEnumList("Education");
            model.GenderList = _interface.FillEnumList("Gender");
            model.OccupationList = _interface.FillEnumList("OccupationType");
            model.TitleList = _interface.FillEnumList("TitleType");
            model.RelationShipList = _interface.FillEnumList("RelationShip");
            model.ModelList = _interface.FillEnumList("ModelType");
            model.ClientFamilyList = _interface.GetDropDownList(Constants.Procedures.ClienFamilyList);
            model.EmployeeList = _interface.GetDropDownList(Constants.Procedures.EmployeeList);
            model.BrokersList = _interface.GetDropDownList(Constants.Procedures.EquityBrokersList);
            model.CountryList = _interface.GetDropDownList(Constants.Procedures.GetCountryDropDown);
            model.MICRList = _interface.GetDropDownList(Constants.Procedures.MICRList);
            model.DepositoryList = _interface.GetDropDownList(Constants.Procedures.DipositoriesList);
            model.AccountTypeList = _interface.GetDropDownList(Constants.Procedures.GetAccountTypesDropDown);
            model.SBUList = _interface.GetDropDownList(Constants.Procedures.SBUList);
            model.BranchList = _interface.GetDropDownList(Constants.Procedures.EntityBranchList);
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Summary view of ClientMain Master
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            ClientMainViewModel model = new ClientMainViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Get the datatable value and render in summary view
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_interface.Summary());
        }

        /// <summary>
        /// Get the model value by ID and pass into the Update View
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(long ID)
        {
            ClientMainViewModel model = new ClientMainViewModel();
            InitAccessModel(model);
            model.ClientDetails = _interface.GetData(ID);
            model.AddressDetails = _interface.GetAddressData(ID);
            model.BankDetails = _interface.GetBankDetailsData(ID);
            model.DepositryDetails = _interface.GetDepositryDetailData(ID);
            model.EquityBrokerDetails = _interface.GetEquityBrokersData(ID);
            model.PersonalDetails = _interface.GetPersonalDetailData(ID);
            model.SecondaryContactDetails = _interface.GetSecondaryContactDetailData(ID);
            model.MappingDetails = _interface.GetMappingDetailData(ID);
            if (model.ClientDetails != null)
            {
                model.StateList = _interface.GetDropDownList(Constants.Procedures.FillStateDropDown);
                model.AccountTypeList = _interface.GetDropDownList(Constants.Procedures.GetAccountTypesDropDown);
                model.CityList = _interface.GetDropDownList(Constants.Procedures.FillCityDropDown);
                model.BankAccountTypeList = _interface.FillEnumList("AccountType");
                model.AnnualIncomeList = _interface.FillEnumList("AnnualIncome");
                model.CitizenShipList = _interface.FillEnumList("Citizenship");
                model.MaterialList = _interface.FillEnumList("MaterialStatus");
                model.ResidentalList = _interface.FillEnumList("ResidentialStatus");
                model.TradingExperienceList = _interface.FillEnumList("TradingExperience");
                model.EducationList = _interface.FillEnumList("Education");
                model.GenderList = _interface.FillEnumList("Gender");
                model.OccupationList = _interface.FillEnumList("OccupationType");
                model.TitleList = _interface.FillEnumList("TitleType");
                model.RelationShipList = _interface.FillEnumList("RelationShip");
                model.ModelList = _interface.FillEnumList("ModelType");
                model.ClientFamilyList = _interface.GetDropDownList(Constants.Procedures.ClienFamilyList);
                model.EmployeeList = _interface.GetDropDownList(Constants.Procedures.EmployeeList);
                model.BrokersList = _interface.GetDropDownList(Constants.Procedures.EquityBrokersList);
                model.CountryList = _interface.GetDropDownList(Constants.Procedures.GetCountryDropDown);
                model.MICRList = _interface.GetDropDownList(Constants.Procedures.MICRList);
                model.DepositoryList = _interface.GetDropDownList(Constants.Procedures.DipositoriesList);
                model.SBUList = _interface.GetDropDownList(Constants.Procedures.SBUList);
                model.BranchList = _interface.GetDropDownList(Constants.Procedures.EntityBranchList);
                return View("Index", model);
            }
            else
            {
                return RedirectToAction("DateNotFound", "Home");
            }
        }

        /// <summary>
        /// Add and Update the ClientMain Master.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(ClientMainViewModel model)
        {
            JsonResponse resp = new JsonResponse();
            // If the model valid go ti the next level
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model));
            } // Else it  shows the error message.
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }
        }

        // [HttpPost]
        //[IgnoreAntiforgeryToken]
        //public IActionResult AddUpdateAccountDetails(List<ClientAccountList> model)
        //{
        //    JsonResponse resp = new JsonResponse(); 
        //    // If the model valid go ti the next level
        //    if (ModelState.IsValid)
        //    {
        //        return Json(_interface.AddUpdateAccountDetails(model));
        //    } // Else it  shows the error message.
        //    else
        //    {
        //        resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
        //        return Json(resp);
        //    }
        //}

        /// <summary>
        /// Change the Status of the data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Json response</returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_interface.Deactivate(ID, Status));
        }

        public IActionResult FillStateList(long CountryID)
        {
            return Json(_interface.GetAddressesDropDowns(Constants.Procedures.GetAddressesDropDowns, CountryID, "State"));
        }

        public IActionResult FillCityList(long StateID)
        {
            return Json(_interface.GetAddressesDropDowns(Constants.Procedures.GetAddressesDropDowns, StateID, "City"));
        }

        public IActionResult IsExsits(string Data,string Flag)
        {
            JsonResponse resp = new JsonResponse();
            if (_interface.IsExsits(Flag, Data)) {
                resp.Status = Constants.ResponseStatus.Success;
                resp.Message = Flag + " already  exists";
            }
            return Json(resp);
        }

        public IActionResult GetAccountsPartialView(int Flag)
        {
            ClientMainViewModel model = new ClientMainViewModel();
            model.AccountTypeList = _interface.GetDropDownList(Constants.Procedures.GetAccountTypesDropDown);
            model.Flag = Flag;
            return PartialView("_AccountsPartialView", model);
        }
    }
}
