using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Services;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{
    public class EquityBrokerMasterController : BaseController
    {
        private readonly IEquityBrokerService _interface;
        [Obsolete]
        private readonly IHostingEnvironment _HostingEnvironment;

        //JsonResponse resp = new JsonResponse();

        [Obsolete]
        public EquityBrokerMasterController(IEquityBrokerService _IEquityBrokerService, IHostingEnvironment HostingEnvironment, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IEquityBrokerService;
            _HostingEnvironment = HostingEnvironment;
        }

        /// <summary>
        /// Add and Update view of the Model
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            EquityBrokersViewModel model = new EquityBrokersViewModel
            {
                CountryList = _interface.GetCountryList(),
                CompanyList=_interface.GetCompanyList()
            };
            InitAccessModel(model);
            return View(model);

        }

        /// <summary>
        /// Summary View of the data from database.
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult SummaryView()
        {
            EquityBrokersViewModel model = new EquityBrokersViewModel();
            InitAccessModel(model);
            return View(model);
        }

        /// <summary>        
        /// Get the Data based on ID and return to the model to update view.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Update(int ID)
        {
            EquityBrokersViewModel model = new EquityBrokersViewModel
            {
                equityBrokers = _interface.GetData(ID)
            };
            if (model.equityBrokers != null)
            {
                model.CompanyList = _interface.GetCompanyList();
                model.CountryList = _interface.GetCountryList();
                model.StateList = _interface.GetStateList(model.equityBrokers.CountryID);
                model.CityList = _interface.GetCityList(model.equityBrokers.StateID);
                InitAccessModel(model);
                return View("Index", model);
            }
            else
            {
                return RedirectToAction("DateNotFound", "Home");
            }

        }

        /// <summary>
        /// Add Update data based on their ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(EquityBrokersViewModel model)
        {
            // If the model valid go to the next level
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.equityBrokers));
            }
            // Else it  shows the error message.
            else
            {
                JsonResponse resp = new JsonResponse
                {
                    Message = Constants.ControllerMessage.All_Fields_Mandatory
                };
                return Json(resp);
            }
        }

        /// <summary>
        /// Check the data was exsits or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Json response</returns>
        public IActionResult IsExstis(string name, long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_interface.IsExsits(name,"Name", ID))
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }
        public IActionResult IsBSERExstis(string name, long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_interface.IsExsits(name,"BSE",ID))
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }
        public IActionResult IsNSEExstis(string name, long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_interface.IsExsits(name,"NSE", ID))
            {
                resp.Status = Constants.ResponseStatus.Success;
            }
            else
            {
                resp.Message = Constants.ControllerMessage.Data_Exsists;
            }
            return Json(resp);
        }
        public IActionResult IsMCXExstis(string name, long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_interface.IsExsits(name,"MCX", ID))
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
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        public IActionResult Summary()
        {
            return Json(_interface.Summary());
        }


        /// <summary>
        /// Get City List to fill Dropdown.
        /// </summary>
        /// <param name="StateID"></param>
        /// <returns></returns>
        public IActionResult GetCityList(long StateID)
        {
            return Json(_interface.GetCityList(StateID));
        }


        /// <summary>
        /// Get State List to fill Dropdown.
        /// </summary>
        /// <param name="CountryID"></param>
        /// <returns></returns>
        public IActionResult GetStateList(long CountryID)
        {
            return Json(_interface.GetStateList(CountryID));
        }


        /// <summary>
        /// Change the Status of the data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Json response</returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_interface.Deactivate(ID, Status));
        }


        /// <summary>
        /// Create the File in the webrootpath.
        /// </summary>
        /// <param name="modal"></param>
        /// <returns>returns the image name.</returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Obsolete]
        public JsonResult UploadLogo(EquityBrokers modal)
        {
            JsonResponse resp = new JsonResponse();
            try
            {
                if (modal != null && modal.LogoFile != null && modal.LogoFile.Length > 0)
                {
                    var filename = Path.Combine(RandomString() + modal.LogoFile.FileName);
                    var path = Path.Combine(_HostingEnvironment.WebRootPath, "Uploaded_Image", filename);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        modal.LogoFile.CopyTo(fileStream);
                    }
                    //  string filePath = Path.Combine(_HostingEnvironment.WebRootPath, "NewFolder", filename);
                    //  modal.LogoFileData = ReadFile(filePath, modal.LogoFile.FileName);
                    resp.Data = filename;
                    resp.Status = Constants.ResponseStatus.Success;

                }
                else if (modal.Logo != null && modal.ID != 0)
                {
                    return Json(_interface.CheckImage(modal));
                }
                else
                {
                    resp.Message = Constants.ControllerMessage.Upload_Needed;
                }
            }
            catch (Exception)
            {
                resp.Message = Constants.ControllerMessage.Upload_Needed;
            }
            return Json(resp);
        }

        private readonly Random _random = new Random();


        /// <summary>
        /// Retuns the Random String to prepend the image file name
        /// </summary>
        /// <returns>String</returns>
        public string RandomString()
        {
            bool lowerCase = false;
            var builder = new StringBuilder(5);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < 5; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        //public static byte[] ReadFile(string filePath, string fileName)
        //{
        //    byte[] buffer;
        //    if (filePath != null && fileName != null)
        //    {
        //        FileStream fileStream1 = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //        try
        //        {
        //            int length = (int)fileStream.Length;  // get file length
        //            buffer = new byte[length];            // create buffer
        //            int count;                            // actual number of bytes read
        //            int sum = 0;                          // total number of bytes read

        //            // read until Read method returns 0 (end of the stream has been reached)
        //            while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
        //                sum += count;  // sum is a buffer offset for next reading
        //        }
        //        finally
        //        {
        //            fileStream.Close();
        //        }
        //        return buffer;
        //    }
        //    else
        //    {
        //        buffer = new byte[0];
        //        buffer.DefaultIfEmpty();
        //        return buffer;
        //    }
        //}


    }
}
