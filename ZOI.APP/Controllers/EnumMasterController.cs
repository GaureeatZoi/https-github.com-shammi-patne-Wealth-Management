using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;
using OfficeOpenXml;
using System.Linq;

namespace ZOI.APP.Controllers
{
    public class EnumMasterController : BaseController
    {
        private readonly IEnumMasterService _interface;

        JsonResponse resp = new JsonResponse();
        [Obsolete]
        public EnumMasterController(IEnumMasterService _IEnumMasterService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IEnumMasterService;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            EnumViewModel model = new EnumViewModel();
            InitAccessModel(model);
            model.EnumMaster.ID = 0;
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(long ID)
        {  // returns Update view

            EnumViewModel model = new EnumViewModel();
            InitAccessModel(model);
            model.EnumMaster = _interface.GetData(ID);
            if (model != null)
            {
                return View("Index", model);
            }
            else
            {
                resp.Message = "";
                return Json(resp);
            }
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult AddUpdate(EnumViewModel model)
        {
            return Json(_interface.AddUpdate(model.EnumMaster));

        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetAll()
        { // gets entity listing data


            return Json(_interface.Summary());
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {   // returns listing view
            EnumViewModel model = new EnumViewModel();
            InitAccessModel(model);
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {  // changes active deacivate status
            return Json(_interface.Deactivate(ID, Status));
        }

        public IActionResult IsExsitsEnumEcode(string EnumType, string EnumCode)
        { // Check uniqueness of Enum Name and Enum Code
            if (!_interface.IsExsitsEnumEcode(EnumType, EnumCode))
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }


        public IActionResult IsExsitsEnumEVal(string EnumType,  int EnumVal)
        { // Check uniqueness of Enum Name and Enum Code
            if (!_interface.IsExsitsEnumEVal(EnumType,  EnumVal))
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }



    }
}
