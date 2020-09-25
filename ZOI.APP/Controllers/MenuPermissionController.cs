using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZOI.APP.Filters;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{

    public class MenuPermissionController : BaseController
    {
        private readonly IRolePermissionService _interface;

        public MenuPermissionController(IRolePermissionService _IRolePermissionService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IRolePermissionService;
        }

        /// <summary>
        /// Role Permission View
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Index()
        {
            RolePermissionViewModel model = new RolePermissionViewModel();
            InitAccessModel(model);
            model.RolesList = _interface.GetRolesData();
            return View(model);
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddPermission(List<MenuPermissionList> model)
        {
            return Json(_interface.AddUpdate(model));
        }


       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult MenuPermissionSummary(int RoleID)
        {
            var resp = _interface.Summary(RoleID);
            return Json(resp);
        }

    }
}
