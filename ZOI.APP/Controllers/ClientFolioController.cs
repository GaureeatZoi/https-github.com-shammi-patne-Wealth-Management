using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.WindowsRuntime;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class ClientFolioController : BaseController
    {
        private readonly IClientFolioService _interface;

        public ClientFolioController(IServiceFactory serviceFactory, IClientFolioService _IClientFolioService) :    base(serviceFactory)
        {
            _interface = _IClientFolioService;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            ClientFolioViewModel model = new ClientFolioViewModel();
            model.AMCList = _interface.FillDropDown(Constants.Procedures.GetAMCDropDownByRTA, null);
            model.CLientList = _interface.FillDropDown(Constants.Procedures.GetClientDropDown, null);
            InitAccessModel(model);
            return View(model);
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Index(ClientFolioViewModel model)
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
            ClientFolioViewModel model = new ClientFolioViewModel();
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
            ClientFolioViewModel model = new ClientFolioViewModel();
            InitAccessModel(model);
            model.clientFolio = _interface.GetClientFolio(ID);
            if (model.clientFolio!=null)
            {               
                model.AMCList = _interface.FillDropDown(Constants.Procedures.GetAMCDropDownByRTA, model.clientFolio.RTAID);
                model.SchemaList = _interface.FillDropDown(Constants.Procedures.GetSchemaDropDownByAMC, model.clientFolio.AMCID);
                model.CLientList = _interface.FillDropDown(Constants.Procedures.GetClientDropDown, model.clientFolio.ClientID);
                return View("Index", model);
            }
            else
            {
                return RedirectToAction("DateNotFound", "Home");
            }           
        }

        public IActionResult FillAccountList(long ClientID, long AccountTypeID)
        {
            return Json(_interface.FillAccountDropDown(Constants.Procedures.GetL5AccountsDropdown, ClientID,AccountTypeID));
        }

         public IActionResult FillAccountTypeList(long ID)
        {
            return Json(_interface.FillDropDown(Constants.Procedures.GetL4AccountTypeDropdown, ID));
        }


        public IActionResult FillSchemaList(long ID)
        {
            return Json(_interface.FillDropDown(Constants.Procedures.GetSchemaDropDownByAMC, ID));
        }
        public IActionResult FillInvestorDetail(long ID)
        {
            return Json(_interface.GetInvestorDetail(ID));
        }

        /// <summary>
        /// Check the data was exsits or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Json response</returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult IsExists(string FolioNo, long SchemaID)
        {
           return Json(_interface.IsExsits(FolioNo, SchemaID));
        }


    }
}
