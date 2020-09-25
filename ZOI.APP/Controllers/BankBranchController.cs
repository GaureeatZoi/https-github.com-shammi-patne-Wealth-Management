using System;
using Microsoft.AspNetCore.Mvc;
using NPOI.XWPF.UserModel;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class BankBranchController : BaseController
    {
        private readonly IBankBranchService _interface;

        //JsonResponse resp = new JsonResponse();

        [Obsolete]
        public BankBranchController(IBankBranchService _IBankBranchService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IBankBranchService;
        }


        /// <summary>
        /// Add and Update view of the Model
        /// </summary>
        /// <returns></returns>
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            BankBranchViewModel model = new BankBranchViewModel();
            model.BankList = _interface.GetBankList();
            model.CountryList = _interface.GetAddressesDropDowns(Constants.Procedures.GetAddressesDropDowns,null,"Country");
            model.bankBranchMaster = new BankBranch
            {
                IsActive = true,
                PinCode = null,
            };
            InitAccessModel(model);
            return View(model);
        }


        /// <summary>
        /// Summary View of the data from database.
        /// </summary>
        /// <returns></returns>
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            BankBranchViewModel model = new BankBranchViewModel();
            InitAccessModel(model);
            return View(model);
        }


        /// <summary>        
        /// Get the Data based on ID and return to the model to update view.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns> 
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(int ID)
        {
            BankBranchViewModel model = new BankBranchViewModel
            {
                bankBranchMaster = _interface.GetData(ID)
            };
            if (model.bankBranchMaster != null)
            {
                model.BankList = _interface.GetBankList();
                model.CountryList = _interface.GetAddressesDropDowns(Constants.Procedures.GetAddressesDropDowns,null, "Country");
                model.StateList = _interface.GetAddressesDropDowns(Constants.Procedures.GetAddressesDropDowns, model.bankBranchMaster.CountryID, "State");
                model.CityList = _interface.GetAddressesDropDowns(Constants.Procedures.GetAddressesDropDowns,model.bankBranchMaster.StateID, "city");
                InitAccessModel(model);
                return View("Index", model);
            }
            else
            {
                return RedirectToAction("DateNotFound", "Home");
            }
        }


        /// <summary>
        /// Add Update data based on their ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(BankBranchViewModel model)
        {
            // If the model valid go ti the next level
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.bankBranchMaster));
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
        /// Check the data was exsits or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Json response</returns>
        public IActionResult IsExstis(string name, long ID, long Bank)
        {
            JsonResponse resp = new JsonResponse();
            if (!_interface.IsExsits(name, ID, Bank))
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
        /// Check the IFSCCode data was exsits or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Json response</returns>
        public IActionResult IsIFSCExstis(string name)
        {
            JsonResponse resp = new JsonResponse();
            if (!_interface.IsIFSCExsits(name))
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
        /// Check the MICRCode data was exsits or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Json response</returns>
        public IActionResult IsMICRExstis(string name)
        {
            JsonResponse resp = new JsonResponse();
            if (!_interface.IsMICRExsits(name))
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
        /// Data summary from the  database.
        /// </summary>
        /// <returns></returns>
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_interface.Summary());
        }



        /// <summary>
        /// Get City List to fill Dropdown.
        /// </summary>
        /// <param name="StateID"></param>
        /// <returns></returns>
        public IActionResult GetCityList(long StateID)
        {
            return Json(_interface.GetAddressesDropDowns(Constants.Procedures.GetAddressesDropDowns, StateID, "City"));
        }


        /// <summary>
        /// Get State List to fill Dropdown.
        /// </summary>
        /// <param name="StateID"></param>
        /// <returns></returns>
        public IActionResult GetStateList(long CountryID)
        {
            return Json(_interface.GetAddressesDropDowns(Constants.Procedures.GetAddressesDropDowns, CountryID, "State"));
        }


        /// <summary>
        /// Change the Status of the data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Json response</returns>
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_interface.Deactivate(ID, Status));
        }

    }
}
