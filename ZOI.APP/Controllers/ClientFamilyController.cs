using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.APP.Models;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class ClientFamilyController : BaseController
    {

        private readonly IClientGroupService _clientGroupService;

        private readonly IClientFamilyService _clientFamilyService;

     


        public ClientFamilyController(IClientGroupService clientGroupService
            , IClientFamilyService clientFamilyService           
            , IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _clientGroupService = clientGroupService;
            _clientFamilyService = clientFamilyService;
        }

        

        /// <summary>
        /// Summary View of the data from database.
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Index()
        {
            ClientFamilyViewModel model = new ClientFamilyViewModel();
            InitAccessModel(model);
            return View(model);
        }


        /// <summary>
        /// Add and Update view of the Model.
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ClientsFamily()
        {
            ClientFamilyViewModel model = new ClientFamilyViewModel
            {
                GroupList = _clientFamilyService.GetDropdownGroups(),
                CountryList = _clientGroupService.GetDropdownCountry(),
            };
            InitAccessModel(model);
            return View(model);

        }


        /// <summary>
        /// Get State List to fill Dropdown.
        /// </summary>
        /// <param name="StateID"></param>
        /// <returns></returns>
        public JsonResult FillStates(int countryID)
        {
            JsonResponse response = new JsonResponse
            {
                Data = _clientGroupService.FillStates(countryID)
            };
            return Json(response);
        }


        /// <summary>
        /// Get City List to fill Dropdown.
        /// </summary>
        /// <param name="StateID"></param>
        /// <returns></returns>
        public JsonResult FillCities(int stateID)
        {
            JsonResponse response = new JsonResponse
            {
                Data = _clientGroupService.FillCities(stateID)
            };
            return Json(response);
        }


        /// <summary>
        /// Add Update data based on their ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(ClientFamilyViewModel model)
        {
            JsonResponse resp = new JsonResponse();
            if (ModelState.IsValid)
            {
                return Json(_clientFamilyService.AddUpdate(model.clientFamily));
            }
            else
            {
                resp.Status = "F";
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }
        }

        /// <summary>
        /// Check the data was exsits or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Json response</returns>
        public IActionResult IsExstis(string name)
        {
            JsonResponse resp = new JsonResponse();
            if (!_clientFamilyService.IsExsits(name))
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
        /// Get the Data based on ID and return to the model to update view.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns> 
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult UpdateClientFamily(int ID)
        {
            ClientFamilyViewModel model = new ClientFamilyViewModel
            {
                clientFamily = _clientFamilyService.GetData(ID)
            };
            if (model.clientFamily != null)
            {
                model.GroupList = _clientFamilyService.GetDropdownGroups();
                model.CountryList = _clientGroupService.GetDropdownCountry();
                model.StateList = _clientGroupService.GetDropdownState(model.clientFamily.CountryID);
                model.CityList = _clientGroupService.GetDropdownCity(model.clientFamily.StateID);
                InitAccessModel(model);
                return View("ClientsFamily", model);
            }
            else
            {
                return RedirectToAction("DateNotFound", "Home");
            }
        }

        /// <summary>
        /// Data summary from the  database.
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IEnumerable<ClientFamily> Summary()
        {            
            IEnumerable<ClientFamily>  clientFamily = _clientFamilyService.Summary();
            return clientFamily;
        }

        /// <summary>
        /// Change the Status of the data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Json response</returns>
        public IActionResult Deactivate(long ID,bool Status)
        {
            return Json(_clientFamilyService.Deactivate(ID, Status));
        }

    }
}
