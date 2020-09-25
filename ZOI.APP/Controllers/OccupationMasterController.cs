using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class OccupationMasterController : BaseController
    {
        private readonly IOccupationService _occupationService;

        public OccupationMasterController(IOccupationService occupationService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _occupationService = occupationService;
           
        }
       

        #region Add,Update,Listing of Occupation Master
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            OccupationViewModel model = new OccupationViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Add update Occupation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(OccupationViewModel model)
        {
            JsonResponse response = new JsonResponse();
            // If the model valid go to the next level
            if (ModelState.IsValid)
            {
                response = _occupationService.AddUpdate(model.OccupationMaster);
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
            OccupationViewModel model = new OccupationViewModel();
              model.OccupationMaster  =_occupationService.GetData(ID);
          
            if (model != null)
            {
                //InitAccessModel(model);
                return View("Index", model);
            }
            else
            {

                return View("SummaryView", model);
            }
        }

        //Check Occupation is already exist or not
        public IActionResult IsExstis(string name, long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_occupationService.IsExsits(name, ID))
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
            OccupationViewModel model = new OccupationViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Get Occupation data
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_occupationService.Summary());
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
            return Json(_occupationService.Deactivate(ID, Status));
        }
        #endregion
    }
}
