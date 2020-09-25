using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.DAL;
using ZOI.DAL.DatabaseUtility;

namespace ZOI.BAL.Services
{
    public class AMCService : IAMCService
    {
        private readonly DatabaseContext _context;

        private readonly IHostingEnvironment _HostingEnvironment;

        JsonResponse resp = new JsonResponse();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AMCService(DatabaseContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment HostingEnvironment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _HostingEnvironment = HostingEnvironment;
        }

        public long GetUserID()
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            return Convert.ToInt64(identity.Claims.Where(c => c.Type == "RoleID")
                  .Select(c => c.Value).SingleOrDefault());
        }
   



    /// <summary>
    /// Add Update Data
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Jsonresponse</returns>
    public JsonResponse AddUpdate(AMCs model)
        {
            try
            {
                //  If these condition true the data was not exsits in the database
                if (!IsExsits(model.Name, model.Id))
                {
                    //  If model.ID == 0 the data goes to the Add part.                
                    if (model.Id == 0)
                    {
                        model.IsActive = true;
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        _context.Set<AMCs>().Add(model);
                        int i = _context.SaveChanges();
                        //  If i != 0 means it affect one data So the data was Inserted.
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_insert_success;
                        }
                        //esle It gives some error in data insertion.
                        else
                        {
                            resp.Message = Constants.Service.Data_insert_failed;
                            var file = Path.Combine(_HostingEnvironment.WebRootPath, "NewFolder", model.Logo);
                            if (File.Exists(file))
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    //  Else data goes to the Update part.
                    else
                    {
                        resp.Message = Constants.Service.Data_Update_failed;
                        var models = GetData(model.Id);
                        // If the models not null it get the database data belongs to the ID 
                        if (models != null)
                        {
                            models.Name = model.Name;
                            models.Address = model.Address;
                            models.IsActive = true;
                            models.ModifiedOn = DateTime.Now;
                            models.ModifiedBy = GetUserID();
                            _context.Set<AMCs>().Update(models);
                            int i = _context.SaveChanges();
                            if (i != 0)
                            {
                                resp.Status = Constants.ResponseStatus.Success;
                                resp.Message = Constants.Service.Data_Update_success;
                            }
                        }
                    }
                }
                // The data was in the database so, It return the else part
                else
                {
                    resp.Message = Constants.ControllerMessage.Data_Exsists;
                }
            }
            catch (Exception)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        /// <summary>
        /// Data Is Exsits
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Boolean values</returns>
        public bool IsExsits(string name, long ID)
        {
            bool IsExsits = true;
            //If the condition true the data with this name doestn't have the duplicate value in the database.
            if (_context.AMC.Where(e => e.Name == name && e.Id != ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;
        }

        /// <summary>
        /// Data Summary
        /// </summary>
        /// <returns>Jsonresponse</returns>
        public JsonResponse Summary()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetAMCsSummary, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new AMCs
                {
                    Id = a.Field<long>("Id")
                   ,
                    Name = a.Field<string>("AMCName")
                   ,
                    Address = a.Field<string>("AMCAddress")
                   ,
                    RTAName = a.Field<string>("RTAName")
                   ,
                    IsActiveText = a.Field<string>("IsActive")
                   ,
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate")
                }).ToList();
            }
            catch
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        /// <summary>
        /// Get the model.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Return  model.</returns>
        public AMCs GetData(long ID)
        {
            try
            {
                if (ID != 0)
                {
                    return _context.AMC.AsNoTracking().Where(e => e.Id == ID).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Change the Status
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse Deactivate(long ID, bool Status)
        {
            try
            {
                //If the ID was not null It goes to the change status part.
                if (ID != 0)
                {
                    resp.Message = Constants.Service.Common_message;
                    var model = GetData(ID);
                    if (model != null)
                    {
                        model.IsActive = Status;
                        model.ModifiedOn = DateTime.Now;
                        model.ModifiedBy = GetUserID();
                        _context.Set<AMCs>().Update(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Status_changed_success;
                        }
                    }
                }
                // Else it Show the error message.
                else
                {
                    resp.Message = Constants.Service.Common_message;
                }
            }
            catch
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        /// <summary>
        /// RTA List.
        /// </summary>
        /// <returns>RTAs List.</returns>
        public IEnumerable<SelectListItem> RTAsList()
        {
            return _context.RTA.AsNoTracking().OrderBy(e => e.Name).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.Name
            }).ToList();
        }

        //public JsonResponse UploadLogo(AMCs modal)
        //{
        //    try
        //    {
        //        if (modal != null)
        //        {
        //            var updateModal = _context.aMCs.Where(e => e.Id == modal.Id).FirstOrDefault();

        //            updateModal.Logo = modal.LogoFile.FileName;
        //            updateModal.ModifiedBy = 1;
        //            updateModal.ModifiedOn = DateTime.UtcNow;
        //            _context.aMCs.Update(updateModal);
        //            if (_context.SaveChanges() > 0)
        //            {
        //                resp.Status = Constants.ResponseStatus.Success;
        //                resp.Message = "Record Updated Successfully";
        //            }
        //            else
        //            {
        //                resp.Status = "F";
        //                resp.Message = "Image not upload. please try again!";
        //            }
        //        }
        //        else
        //        {
        //            resp.Status = "F";
        //            resp.Message = "something went wrong !";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        StackTrace CallStack = new StackTrace(ex, true);
        //        ex.Data["ErrDescription"] = ex.Data["ErrDescription"] != null ? ex.Data["ErrDescription"] : string.Format("Error captured in {0} on Line No {1} of Method {2}", CallStack.GetFrame(0).GetFileName(), CallStack.GetFrame(0).GetFileLineNumber(), CallStack.GetFrame(0).GetMethod().ToString());
        //        throw ex;
        //    }
        //    return resp;
        //}

        /// <summary>
        /// Check Image in the database
        /// </summary>
        /// <param name="modal"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse CheckImage(AMCs model)
        {
            //Check the Image file name in the model and return the values if the condition true it retun the model logo name.
            if (_context.AMC.Where(e => e.Logo == model.Logo).FirstOrDefault() != null)
            {
                resp.Status = Constants.ResponseStatus.Success;
                resp.Data = model.Logo;
            }
            // Else the error function was trigger and send the message.
            else
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;

        }

    }
}
