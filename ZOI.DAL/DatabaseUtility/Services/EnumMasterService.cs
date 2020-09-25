using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

using ZOI.DAL.DatabaseUtility.Interface;
using ZOI.BAL.Utilites;

using ZOI.BAL.DBContext;
using ZOI.BAL;

using System.Security.Claims;
using System.Linq;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class EnumMasterService : IEnumMasterService
    {
        private readonly DatabaseContext _context;

        private readonly IHostingEnvironment _HostingEnvironment;

        JsonResponse resp = new JsonResponse();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public EnumMasterService(DatabaseContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment HostingEnvironment)
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
        public JsonResponse AddUpdate(ZOI.BAL.Models.Enum model)
        {
           
                //  If these condition true the data was not exsits in the database
               
                    //  If model.ID == 0 the data goes to the Add part.                
                    if (model.ID == 0)
                    {
                    if (!IsExsitsEnumEcode(model.EnumType, model.EnumCode) && !IsExsitsEnumEVal(model.EnumType, model.EnumValue))
                    {
                        try
                        {
                            SqlParameter[] ObjParams = new SqlParameter[] {
                         new SqlParameter("@EnumType",model.EnumType),
                         new SqlParameter("@EnumCode", model.EnumCode),
                          new SqlParameter("@EnumValue", model.EnumValue),
                         new SqlParameter("@EnumDescription", model.EnumDescription),
                         new SqlParameter("@IsActive", model.IsActive),
                         new SqlParameter("@Icons", model.Icons),
                         new SqlParameter("@CreatedBy",GetUserID())
                    };

                            new ADODataFunction().ExecuteNonQuery(Constants.Procedures.InsertEnum, ObjParams, CommandType.StoredProcedure);
                            ObjParams = null;
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_insert_success;
                        }
                        catch (Exception ex)
                        {
                            resp.Status = Constants.ResponseStatus.Failed;
                            resp.Message = Constants.Service.Data_insert_failed;

                        }

                    }
                    // The data was in the database so, It return the else part
                    else
                    {
                        resp.Message = Constants.ControllerMessage.Data_Exsists;
                    }



                    }
                    //  Else data goes to the Update part.
                    else
                    {
                        resp.Message = Constants.Service.Data_Update_failed;
                        var models = GetData(model.ID);
                        // If the models not null it get the database data belongs to the ID 
                        if (models != null)
                        {  
                           
                            
                            try
                            {
                                SqlParameter[] ObjParams = new SqlParameter[] {
                         new SqlParameter("@ID",model.ID),

                         new SqlParameter("@EnumType",model.EnumType),
                         new SqlParameter("@EnumCode", model.EnumCode),
                           new SqlParameter("@EnumValue", model.EnumValue),
                         new SqlParameter("@EnumDescription", model.EnumDescription),
                         new SqlParameter("@IsActive", model.IsActive),
                         new SqlParameter("@Icons", model.Icons),
                         new SqlParameter("@ModifiedBy",GetUserID())
                    };

                                new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateEnum, ObjParams, CommandType.StoredProcedure);
                                ObjParams = null;
                                resp.Status = Constants.ResponseStatus.Success;
                                resp.Message = Constants.Service.Data_Update_success;
                            }
                            catch (Exception ex)
                            {
                                resp.Status = Constants.ResponseStatus.Failed;
                                resp.Message = Constants.Service.Data_Update_failed;

                            }
                        }
                    }
                
               return resp;
            }
            
            
        //}

        /// <summary>
        /// Data Is Exsits
        /// </summary>
        /// <param name="EnumType"></param>
        /// <param name="EnumNumCode"></param>
        /// <returns>Boolean values</returns>
        public bool IsExsitsEnumEcode(string EnumType, string EnumNumCode)
        {
            bool IsExsits = true;
            //If the condition true the data with this name doestn't have the duplicate value in the database.
            if (_context.Enum.Where(e => e.EnumType == EnumType && e.EnumCode == EnumNumCode ).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;
        }



        /// <summary>
        /// Data Is Exsits
        /// </summary>
        /// <param name="EnumType"></param>
        /// <param name="EnumNumCode"></param>
        /// <returns>Boolean values</returns>
        public bool IsExsitsEnumEVal(string EnumType,  int EnumVal)
        {
            bool IsExsits = true;
            //If the condition true the data with this name doestn't have the duplicate value in the database.
            if (_context.Enum.Where(e => e.EnumType == EnumType &&  e.EnumValue == EnumVal).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;
        }




        /// <summary>
        /// Get the model.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Return  model.</returns>
        public ZOI.BAL.Models.Enum GetData(long ID)
        {
            try
            {
                if (ID != 0)
                {
                    return _context.Enum.AsNoTracking().Where(e => e.ID == ID).FirstOrDefault();
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
                        _context.Set<ZOI.BAL.Models.Enum>().Update(model);
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
        /// Display Summary
        /// </summary>
        /// <param name="ID"></param>
        public JsonResponse Summary()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetEnumList, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new ZOI.BAL.Models.Enum
                {
                    ID = a.Field<long>("Id"),
                    EnumType = a.Field<string>("EnumType"),
                    EnumValue= a.Field<int>("EnumValue"),
                    EnumCode = a.Field<string>("EnumCode"),
                   EnumDescription = a.Field<string>("EnumDescription"),
                    Icons = a.Field<string>("Icons"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;

        }

        public IEnumerable<ZOI.BAL.Models.Enum> ListAll()
        {
            //Used for Employee Listing 
            //Stored Procedure Used for listing  Admin_GetEmployee
            DataSet data = new DataSet();
            IEnumerable<ZOI.BAL.Models.Enum> EnumList;
            try
            {

                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetEnumList, null, CommandType.StoredProcedure);

                EnumList = data.Tables[1].AsEnumerable().Select(a => new ZOI.BAL.Models.Enum
                {
                    ID = a.Field<long>("Id"),
                    EnumType = a.Field<string>("EnumType"),
                    EnumValue = a.Field<int>("EnumValue"),
                    EnumCode = a.Field<string>("EnumCode"),
                    EnumDescription = a.Field<string>("EnumDescription"),
                    Icons = a.Field<string>("Icons"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();

            }
            catch
            {
                return null;
            }

            return EnumList;

        }


    }
}
