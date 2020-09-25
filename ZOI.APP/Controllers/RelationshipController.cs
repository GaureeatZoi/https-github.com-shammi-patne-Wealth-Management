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
    public class RelationshipController : BaseController
    {
        private readonly IRelationshipService _relationshipService;
        public RelationshipController(IRelationshipService relationshipService,IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _relationshipService = relationshipService;
        }

        #region Add,Update,Listing of Relationship Master

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            RelationshipViewModel model = new RelationshipViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Add update Relationship
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(RelationshipViewModel model)
        {
            JsonResponse response = new JsonResponse();
            // If the model valid go to the next level
            if (ModelState.IsValid)
            {
                response = _relationshipService.AddUpdate(model.Relationship);
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
            RelationshipViewModel model = new RelationshipViewModel();
            model.Relationship = _relationshipService.GetData(ID);

            if (model != null)
            {
                return View("Index", model);
            }
            else
            {

                return View("SummaryView", model);
            }
        }
        /// <summary>
        /// check Relationship name is already exist or not
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IActionResult IsExstis(string name, int ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_relationshipService.IsExsits(name, ID))
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
            RelationshipViewModel model = new RelationshipViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Get Relationship data
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_relationshipService.Summary());
        }

        /// <summary>
        /// Change the Status of the data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Json response</returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(int ID, bool Status)
        {
            return Json(_relationshipService.Deactivate(ID, Status));
        }
        #endregion
    }
}
