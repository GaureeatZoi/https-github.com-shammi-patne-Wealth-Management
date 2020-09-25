using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;
using ZOI.DAL.DatabaseUtility.Services;

namespace ZOI.APP.Controllers
{
    
    public class AssetClassMasterController : BaseController
    {
        private readonly IAssetClassService _interface;

        private IServiceFactory _IServiceFactory;

        JsonResponse resp = new JsonResponse();

        public AssetClassMasterController(IAssetClassService _IAssetClassService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IAssetClassService;
            _IServiceFactory = serviceFactory;
        }

        /// <summary>
        /// Add and Update view of the Model
        /// </summary>
        /// <returns></returns>
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            AssetClassViewModel model = new AssetClassViewModel();
            InitAccessModel(model);
            return View(model);
        }


        /// <summary>
        /// Summary View of the data from database.
        /// </summary>
        /// <returns></returns>
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] {"Read"})]
        public IActionResult SummaryView()
        {
            AssetClassViewModel model = new AssetClassViewModel();
            InitAccessModel(model);
            return View(model);
        }

       

        /// <summary>        
        /// Get the Data based on ID and return to the model to update view.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] {"Write"})]
        public IActionResult Update(int ID)
        {
            AssetClassViewModel model = new AssetClassViewModel();
            InitAccessModel(model);
            model.assetClass = _interface.GetData(ID);
            if (model.assetClass != null)
            {
                return View("Index", model);
            }
            else
            {

                return RedirectToAction("DateNotFound", "Home");
            }           
        }

        /// <summary>        
        /// Get the Data based on ID and return to the model to update view.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(AssetClassViewModel model)
        {
            // If the model valid go ti the next level
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.assetClass));
            }// Else it  shows the error message.
            else
            {
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
        public IActionResult IsExstis(string Name, long ID)
        {
            if (!_interface.IsExsits(Name, ID))
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
