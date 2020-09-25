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
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class TransactionTypeController : BaseController
    {

        private readonly ITransactionTypeService _interface;

        public TransactionTypeController(IServiceFactory serviceFactory, ITransactionTypeService _IClientFolioService) : base(serviceFactory)
        {
            _interface = _IClientFolioService;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            TransactionTypeViewModel model = new TransactionTypeViewModel();
            InitAccessModel(model);
            model.TransactionNatureList = new List<SelectListItem>() {
                new SelectListItem {
                    Text = "Sell", Value = "S"
                },
                new SelectListItem {
                    Text = "Buy", Value = "B"
                }
            };
            return View(model);
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Index(TransactionTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model));
            }
            else
            {
                JsonResponse resp = new JsonResponse();
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }

        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            TransactionTypeViewModel model = new TransactionTypeViewModel();
            InitAccessModel(model);
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_interface.Summary());
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_interface.ChangeStatus(ID, Status));
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(long ID)
        {
            TransactionTypeViewModel model = new TransactionTypeViewModel();
            InitAccessModel(model);
            model.transactionTypes = _interface.GetTransactionType(ID);
            if (model.transactionTypes!= null)
            {
                model.TransactionNatureList = new List<SelectListItem>() {
                new SelectListItem {
                    Text = "Sell", Value = "S"
                },
                new SelectListItem {
                    Text = "Buy", Value = "B"
                }
            };
                return View("Index", model);
            }
            else
            {
                return View("SummaryView", model);
            }
        }


    }
}
