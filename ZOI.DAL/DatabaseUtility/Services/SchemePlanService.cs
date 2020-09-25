using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using ZOI.BAL;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.DAL;
using ZOI.DAL.DatabaseUtility;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class SchemePlanService : ISchemePlanService
    {
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonResponse resp = new JsonResponse();

        private readonly IConfiguration _config;

        public SchemePlanService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
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
        /// Add Update Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Jsonresponse</returns>
        public JsonResponse AddUpdate(SchemePlan model)
        {
            try
            {
                //  If these condition true the data was not exsits in the database
                if (!IsExsits(model.Name, model.ID))
                {
                    //  If model.ID == 0 the data goes to the Add part.                
                    if (model.ID == 0)
                    {
                        model.IsActive = true;
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        model.ModifiedOn = DateTime.Now;
                        model.ModifiedBy = GetUserID();
                        _context.Set<SchemePlan>().Add(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_insert_success;
                        }
                        else
                        {
                            resp.Message = Constants.Service.Data_insert_failed;
                        }
                    }
                    //  Else data goes to the Update part.
                    else
                    {
                        resp.Message = Constants.Service.Data_Update_failed;
                        var models = GetData(model.ID);
                        if (models != null)
                        {

                            models.Name = model.Name;

                            models.IsActive = model.IsActive;
                            models.ModifiedOn = DateTime.Now;
                            models.ModifiedBy = GetUserID();
                            _context.Set<SchemePlan>().Update(models);
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
            if (_context.SchemePlans.Where(e => e.Name == name && e.ID != ID).FirstOrDefault() == null)
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
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetSchemePlan, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new SchemePlan
                {
                    ID = a.Field<long>("Id")
                   ,
                    Name = a.Field<string>("Name")
                   ,

                    IsActiveText = a.Field<string>("Status")
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
        public SchemePlan GetData(long ID)
        {
            try
            {
                if (ID != 0)
                {
                    return _context.SchemePlans.AsNoTracking().Where(e => (e.ID) == (ID)).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
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
                    var model = GetData(ID);
                    if(model != null)
                    { 
                    model.IsActive = Status;
                    model.ModifiedOn= DateTime.Now;
                    model.ModifiedBy = GetUserID();
                    _context.Set<SchemePlan>().Update(model);
                    int i = _context.SaveChanges();
                    if (i != 0)
                    {
                        resp.Status = Constants.ResponseStatus.Success;
                        resp.Message = Constants.Service.Status_changed_success;
                    }
                    else
                    {
                        resp.Status = Constants.ResponseStatus.Failed;
                        resp.Message = Constants.Service.Data_Update_failed;
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
    }
}
