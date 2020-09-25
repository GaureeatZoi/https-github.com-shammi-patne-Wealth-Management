using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Validation;
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
    public class ClientGroupController : BaseController
    {

        private readonly IClientGroupService _clientGroupService;
        public ClientGroupController(IClientGroupService clientGroupService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _clientGroupService = clientGroupService;
        }

        /// <summary>
        /// Summary View of the data from database.
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Index()
        {
            ClientGroupViewModel model = new ClientGroupViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Add and Update view of the Model
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ClientGroup()
        {
            ClientGroupViewModel model = new ClientGroupViewModel
            {
                CountryList = _clientGroupService.GetDropdownCountry()
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
        public IActionResult AddUpdate(ClientGroupViewModel model)
        {
            JsonResponse resp = new JsonResponse();
            if (ModelState.IsValid)
            {
                return Json(_clientGroupService.AddUpdate(model.clientGroup));
            }
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }
        }

        /// <summary>        
        /// Get the Data based on ID and return to the model to update view.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult UpdateClientGroup(int ID)
        {
            ClientGroupViewModel model = new ClientGroupViewModel();
            InitAccessModel(model);
            model.clientGroup = _clientGroupService.GetData(ID);
            if (model.clientGroup != null)
            {
                model.CountryList = _clientGroupService.GetDropdownCountry();
                model.StateList = _clientGroupService.GetDropdownState(model.clientGroup.CountryID);
                model.CityList = _clientGroupService.GetDropdownCity(model.clientGroup.StateID);
                return View("ClientGroup", model);
            }
            else
            {
               
                return View("Index", model);
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
            if (!_clientGroupService.IsExsits(name))
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
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IEnumerable<ClientGroup> Summary()
        {
            IEnumerable<ClientGroup> clientGroup = _clientGroupService.Summary();
            return clientGroup;
        }

        /// <summary>
        /// Change the Status of the data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Json response</returns>
        public IActionResult Deactivate(long ID,bool Status)
        {
            return Json(_clientGroupService.Deactivate(ID, Status));
        }
    }
}
