using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class BankService : IBankService
    {
        private readonly DatabaseContext _context;

        private readonly IHostingEnvironment _HostingEnvironment;

        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonResponse resp = new JsonResponse();

        public BankService(DatabaseContext context, IHostingEnvironment HostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _HostingEnvironment = HostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetUserID()
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            return Convert.ToInt64(identity.Claims.Where(c => c.Type == "RoleID")
                  .Select(c => c.Value).SingleOrDefault());
        }

        /// <summary>
        /// Add Update Bank
        /// </summary>
        /// <param name="model"> Add and Update the AMCs.</param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse AddUpdate(Bank model)
        {
            try
            {
                //  If these condition true the data was not exsits in the database
                if (!IsExsits(model.Name, model.Id))
                {
                    //  If model.ID == 0 the data goes to the Add part.                
                    if (model.Id == 0)
                    {
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        _context.Set<Bank>().Add(model);
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
                        var models = GetData(model.Id);
                        // If the models not null it get the database data belongs to the ID 
                        if (models != null)
                        {
                            models.Name = model.Name;
                            models.IsActive = model.IsActive;
                            models.ModifiedOn = DateTime.Now;
                            models.ModifiedBy = GetUserID();
                            _context.Set<Bank>().Update(models);
                            int i = _context.SaveChanges();
                            if (i != 0)
                            {
                                resp.Status = Constants.ResponseStatus.Success;
                                resp.Message = Constants.Service.Data_Update_success;
                            }
                            else
                            {
                                resp.Message = Constants.Service.Data_insert_failed;
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
        /// Change the Status
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse Deactivate(long Id, bool Status)
        {
            try
            {
                //If the ID was not null It goes to the change status part.
                if (Id != 0)
                {
                    resp.Message = Constants.Service.Common_message;
                    var model = GetData(Id);
                    if (model != null)
                    {
                        model.IsActive = Status;
                        model.ModifiedOn = DateTime.Now;
                        model.ModifiedBy = GetUserID();
                        _context.Set<Bank>().Update(model);
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
        /// Get the Bank model.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Return Bank model.</returns>
        public Bank GetData(long Id)
        {
            try
            {
                if (Id != 0)
                {
                    return _context.Bank.AsNoTracking().Where(e => e.Id == Id).FirstOrDefault();
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
        /// Data Is Exsits
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <returns>Boolean values</returns>
        public bool IsExsits(string name, long ID)
        {
            bool IsExsits = true;
            //If the condition true the data with this name doestn't have the duplicate value in the database.
            if (_context.Bank.Where(e => e.Name == name && e.Id != ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;

        }

        /// <summary>
        /// AMCs Summary
        /// </summary>
        /// <returns>Jsonresponse</returns>
        public JsonResponse Summary()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetBankSummary, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new Bank
                {
                    Id = a.Field<long>("Id")
                   ,
                    Name = a.Field<string>("BankName")
                   ,
                    IsActiveText = a.Field<string>("IsActive")
                   ,
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate")
                }).ToList();
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        /// <summary>
        /// Check Image in the database
        /// </summary>
        /// <param name="modal"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse CheckImage(Bank modal)
        {
            //Check the Image file name in the model and return the values if the condition true it retun the model logo name.
            if (_context.Bank.Where(e => e.Logo == modal.Logo).FirstOrDefault() != null)
            {
                resp.Status = Constants.ResponseStatus.Success;
                resp.Data = modal.Logo;
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
