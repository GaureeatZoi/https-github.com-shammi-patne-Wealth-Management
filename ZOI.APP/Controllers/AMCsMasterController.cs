using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.Record;
using NPOI.SS.Formula.Functions;
using ZOI.APP.Filters;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.APP.Controllers
{

    public class AMCsMasterController : BaseController
    {
        private readonly IAMCService _interface;

        private readonly IServiceFactory _IServiceFactory;

        JsonResponse resp = new JsonResponse();

        [Obsolete]
        private readonly IHostingEnvironment _HostingEnvironment;

        [Obsolete]
        public AMCsMasterController(IAMCService _IAMCService, IHostingEnvironment HostingEnvironment, IServiceFactory serviceFactory) : base(serviceFactory)
        {
            _interface = _IAMCService;
            _HostingEnvironment = HostingEnvironment;
        }

        /// <summary>
        /// Add and Update view of the Model
        /// </summary>
        /// <returns></returns>
       // [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult Index()
        {
            AMCsViewModel model = new AMCsViewModel
            {
                RTAsList = _interface.RTAsList()
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
            AMCsViewModel model = new AMCsViewModel();
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
            AMCsViewModel model = new AMCsViewModel
            {
                aMCs = _interface.GetData(ID),
                RTAsList = _interface.RTAsList()
            };
            InitAccessModel(model);
            if (model.aMCs != null)
            {
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
      //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddUpdate(AMCsViewModel model)
        {
            JsonResponse resp = new JsonResponse();
            // If the model valid go ti the next level
            if (ModelState.IsValid)
            {
                return Json(_interface.AddUpdate(model.aMCs));
            } // Else it  shows the error message.
            else
            {
                resp.Message = Constants.ControllerMessage.All_Fields_Mandatory;
                return Json(resp);
            }
        }

        /// <summary>
        /// Check the data was exsits or not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Json response</returns>
      //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult IsExstis(string name, long ID)
        {
            JsonResponse resp = new JsonResponse();
            if (!_interface.IsExsits(name, ID))
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
      //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
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
      //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Write" })]
        public IActionResult ChangeStatus(long ID, bool Status)
        {
            return Json(_interface.Deactivate(ID, Status));
        }

        /// <summary>
        /// Create the File in the webrootpath.
        /// </summary>
        /// <param name="modal"></param>
        /// <returns>returns the image name.</returns>
      //  [TypeFilter(typeof(AuthorizeAction), Arguments = new object[] { "Read" })]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public JsonResult UploadLogo(AMCs modal)
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
                    string filePath = Path.Combine(_HostingEnvironment.WebRootPath, "Uploaded_Image", filename);
                    //modal.LogoFileData = ReadFile(filePath, modal.LogoFile.FileName);
                    resp.Data = filename;
                    resp.Status = Constants.ResponseStatus.Success;
                }
                else if (modal.Logo != null && modal.Id != 0)
                {
                    return Json(_interface.CheckImage(modal));
                }
                else
                {
                    resp.Message = Constants.ControllerMessage.Upload_Needed;
                }
            }
            catch
            {
                resp.Message = Constants.ControllerMessage.Upload_Failed;
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
