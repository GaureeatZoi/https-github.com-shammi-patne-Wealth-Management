using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZOI.APP.Filters;
using ZOI.APP.Models;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController( IServiceFactory serviceFactory): base(serviceFactory)
        {
            
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] {""})]
        public IActionResult Index()
        {
            BaseViewModel model = new BaseViewModel();
            InitAccessModel(model);
            return View(model);
        }



       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] {""})]
        public IActionResult NotAuthorize()
        {
            BaseViewModel model = new BaseViewModel();
            InitAccessModel(model);
            return View(model);
        }

       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] {""})]
        public IActionResult DateNotFound()
        {
            BaseViewModel model = new BaseViewModel();
            InitAccessModel(model);
            return View(model);
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
