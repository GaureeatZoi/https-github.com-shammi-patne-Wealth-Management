using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;
using OfficeOpenXml;
using System.Linq;

namespace ZOI.APP.Controllers
{
    public class SchemeNAVController : BaseController
    {
        private readonly ISchemeNAVService _interface;

        JsonResponse resp = new JsonResponse();

        public SchemeNAVController(ISchemeNAVService _ISchemeNAVService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _ISchemeNAVService;
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {   // returns listing view
            SchemeNAVViewModel model = new SchemeNAVViewModel();
            InitAccessModel(model);
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult GetAll()
        { // gets  listing data


            return Json(_interface.Summary());
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
