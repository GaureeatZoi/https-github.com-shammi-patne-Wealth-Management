using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;


namespace ZOI.APP.Controllers
{
    public class ClientAccountMappingController : BaseController
    {
        private readonly IClientAccountMappingService _interface;

        public ClientAccountMappingController(IClientAccountMappingService _IAccountMappingDetails, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IAccountMappingDetails;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            ClientAccountMappingViewModel model = new ClientAccountMappingViewModel();
            InitAccessModel(model);
            model.ClientList = _interface.ClientList();
            model.AccountTypeList = _interface.AccountTypeList();            
            model.EmployeList = _interface.EmployeeList();            
            return View(model);
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            ClientAccountMappingViewModel model = new ClientAccountMappingViewModel();
            InitAccessModel(model);
           return View(model);
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(long ID)
        {
            ClientAccountMappingViewModel model = new ClientAccountMappingViewModel();
            model.AccountsMapping = _interface.GetDataByID(ID);
            model.ClientList = _interface.ClientList();
            model.AccountTypeList = _interface.AccountTypeList();
            model.EmployeList = _interface.EmployeeList();
            InitAccessModel(model);
            return View("Index",model);
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_interface.Summary());
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(ClientAccountMappingViewModel model)
        {
            JsonResponse resp = new JsonResponse();
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.AccountsMapping));
            }
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }           
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult IsExists(long ClientID, int AccountType ,string UCC)
        {
            return Json(_interface.IsExists(ClientID,AccountType,UCC));
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_interface.ChangeStatus(ID, Status));
        }


    }
}
