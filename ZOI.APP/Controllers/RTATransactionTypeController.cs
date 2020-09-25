using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.BAL.Models;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class RTATransactionTypeController : BaseController
    {

        private readonly IRTATransactionTypeService _interface;

        public RTATransactionTypeController(IServiceFactory serviceFactory, IRTATransactionTypeService _IRTATransactionTypeService) : base(serviceFactory)
        {
            _interface = _IRTATransactionTypeService;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            RTATransactionTypeViewModel model = new RTATransactionTypeViewModel();
            InitAccessModel(model);
            model.RTAList = _interface.FillDropDown(Constants.Procedures.GetRTADropDown);
            model.TransactionTypeList = _interface.FillDropDown(Constants.Procedures.GetTransactionTypeDropDown);
            return View(model);
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Index(RTATransactionTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.RTATransactionTypes));
            }
            else
            {
                JsonResponse resp = new JsonResponse
                {
                    Message = Constants.ControllerMessage.All_Fields_Mandatory
                };
                return Json(resp);
            }
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public  IActionResult MapData(List<TransactionTypeMappingList> model)
        {
            return Json(_interface.MapData(model));
        }






       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            RTATransactionTypeViewModel model = new RTATransactionTypeViewModel();
            InitAccessModel(model);
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult MapTransactionType()
        {
            RTATransactionTypeViewModel model = new RTATransactionTypeViewModel();
            InitAccessModel(model);
           
            return View(model);

        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_interface.Summary());
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult UnmappedSummary()
        {
            var resp = Json(_interface.UnmappedSummary());
            return resp;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_interface.ChangeStatus(ID, Status));
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(long ID)
        {
            RTATransactionTypeViewModel model = new RTATransactionTypeViewModel();
            InitAccessModel(model);
            model.RTATransactionTypes = _interface.GetTransactionType(ID);
            if (model.RTATransactionTypes != null)
            {

                model.RTAList = _interface.FillDropDown(Constants.Procedures.GetRTADropDown);
                model.TransactionTypeList = _interface.FillDropDown(Constants.Procedures.GetTransactionTypeDropDown);
                return View("Index", model);
            }
            else
            {
                return View("SummaryView", model);
            }
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult GetTransactionTypeList()
        { //  dropdown transaction type
            return Json(_interface.FillDropDown(Constants.Procedures.GetTransactionTypeDropDown));
        }

        public IActionResult IsExists(long RTAID,string RTAType,long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (_interface.IsExsits(RTAID, RTAType,ID))
            {
                resp.Status = Constants.ResponseStatus.Failed;
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            else
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            return Json(resp);
        }

    }
}
