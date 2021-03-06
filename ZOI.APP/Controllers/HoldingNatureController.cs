﻿using System;
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
    public class HoldingNatureController : BaseController
    {
        private readonly IHoldingNatureService _holdingNatureService;
        public HoldingNatureController(IHoldingNatureService holdingNatureService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _holdingNatureService = holdingNatureService;
        }

        #region Add,Update,Listing of Holding Nature Master
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            HoldingNatureViewModel model = new HoldingNatureViewModel();
            InitAccessModel(model);
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            HoldingNatureViewModel model = new HoldingNatureViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Get HoldingNature data
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_holdingNatureService.Summary());
        }

        /// <summary>
        /// Add update HoldingNature
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(HoldingNatureViewModel model)
        {
            JsonResponse response = new JsonResponse();
            // If the model valid go to the next level
            if (ModelState.IsValid)
            {
                response = _holdingNatureService.AddUpdate(model.HoldingNature);
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
            HoldingNatureViewModel model = new HoldingNatureViewModel();
            model.HoldingNature = _holdingNatureService.GetData(ID);

            if (model != null)
            {
                return View("Index", model);
            }
            else
            {

                return View("SummaryView", model);
            }
        }

        //Check Hncode is already exist or not
        public IActionResult IsExstis(string name, long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_holdingNatureService.IsExsits(name, ID))
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
        /// Change the Status of the data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Json response</returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_holdingNatureService.Deactivate(ID, Status));
        }
        #endregion
    }
}
