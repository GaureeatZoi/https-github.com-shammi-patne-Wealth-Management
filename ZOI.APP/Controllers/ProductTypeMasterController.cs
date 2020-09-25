using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class ProductTypeMasterController : BaseController
    {
        private readonly IProductTypeService _interface;

        JsonResponse resp = new JsonResponse();

        public ProductTypeMasterController(IProductTypeService _IProductTypeService, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IProductTypeService;
        }

        /// <summary>
        /// Summary View of the data from database.
        /// </summary>
        /// <returns></returns>
        public IActionResult SummaryView()
        {
            ProductTypeViewModel model = new ProductTypeViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>
        /// Add and Update view of the Model
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ProductTypeViewModel model = new ProductTypeViewModel();
            model.productTypeMaster.IsActive = true;
            InitAccessModel(model);
            return View(model);

        }

        /// <summary>        
        /// Get the Data based on ID and return to the model to update view.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public IActionResult Update(int ID)
        {
            ProductTypeViewModel model = new ProductTypeViewModel();
            InitAccessModel(model);
            if (model.CurrentMenuPermission != null)
            {
                if (model.CurrentMenuPermission.Read && model.CurrentMenuPermission.Write)
                {
                    model.productTypeMaster = _interface.GetData(ID);
                    if (model.productTypeMaster != null)
                    {
                        return View("Index", model);
                    }
                    else
                    {
                        if (model.CurrentMenuPermission.Read)
                        {
                            return View("SummaryView", model);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else if (model.CurrentMenuPermission.Read)
                {
                    return View("SummaryView", model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Add Update data based on their ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(ProductTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.productTypeMaster));
            }
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }
        }

        /// <summary>
        /// Check the name data was exsits or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Json response</returns>
        public IActionResult IsExstis(string Name, long ID)
        {
            if (!_interface.IsExsits(Name, ID))
            {
                resp.Status = Constants.ResponseStatus.Success;
                resp.Message = "";
            }
            else
            {
                resp.Status = "F";
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }

        /// <summary>
        /// Check the product code was exsits or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Json response</returns>
        public IActionResult IsCodeExstis(string Code, long ID)
        {
            if (!_interface.IsCodeExstis(Code, ID))
            {
                resp.Status = Constants.ResponseStatus.Success;
                resp.Message = "";
            }
            else
            {
                resp.Status = "F";
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }

        /// <summary>
        /// Data summary from the  database.
        /// </summary>
        /// <returns></returns>
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
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_interface.Deactivate(ID, Status));
        }
    }
}
