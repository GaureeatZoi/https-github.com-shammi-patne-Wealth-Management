using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.DAL.DatabaseUtility;
using System.Data.SqlClient;
using ZOI.DAL;
using ZOI.BAL.DBContext;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ZOI.BAL.Services
{
    public class StateService : IStateService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DatabaseContext _context;

        JsonResponse resp = new JsonResponse();

        public StateService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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

        public IEnumerable<SelectListItem> InitStateView()
        {
            //Used for country dropdown
           
            return _context.Country.AsNoTracking().OrderBy(e => e.CountryName).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CountryName
            }).ToList();
         
        }

        public bool IsStateExists(string StateName)
        {
            bool IsExsits = true;
            //If the condition true the data with this name doestn't have the duplicate value in the database.
            if (_context.State.Where(e => e.StateName == StateName).FirstOrDefault() == null)
            {
                IsExsits = false;
            }

            return IsExsits;
        }

        public bool IsgstcodeExists(int name)
        {
            bool IsExsits = true;
            //If the condition true the data with this gscode  doestn't have the duplicate value in the database.

            if (_context.State.Where(e => e.GSTStateCode == name).FirstOrDefault() == null && name != 0)
            {
                IsExsits = false;
            }

            return IsExsits;

        }

        public JsonResponse Summary()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetStateList, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new State
                {
                    Id = a.Field<long>("Id"),
                    StateName = a.Field<string>("StateName"),
                    StateCode = a.Field<string>("StateCode"),
                    GSTStateCode = a.Field<int>("GSTStateCode"),
                    CountryId = a.Field<long>("CountryId"),
                    CountryName = a.Field<string>("CountryName"),

                    IsUnionTerritory = a.Field<bool>("IsUnionTerritory"),
                    IsActiveText = a.Field<string>("Status"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate")
                }).ToList();
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }



        public IEnumerable<State> ListAll()
        {
            //Used for unique State Master Listing
            //Stored Procedure Used  SP_GetState

            DataSet data = new DataSet();
            IEnumerable<State> StateList;
            try
            {
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetStateList, null, CommandType.StoredProcedure);
                StateList = data.Tables[1].AsEnumerable().Select(dataRow => new State
                {
                    Id = dataRow.Field<long>("Id"),
                    StateName = dataRow.Field<string>("StateName"),
                    StateCode = dataRow.Field<string>("StateCode"),
                    GSTStateCode = dataRow.Field<int>("GSTStateCode"),
                    CountryId = dataRow.Field<long>("CountryId"),
                    CountryName = dataRow.Field<string>("CountryName"),
                    IsUnionTerritory = dataRow.Field<bool>("IsUnionTerritory"),
                    IsActiveText = dataRow.Field<string>("Status"),
                    LastUpdatedDate = dataRow.Field<string>("LastUpdatedDate")
                }).ToList();
                return StateList;
            }
            catch (Exception ex)
            {
                return null;    
            }
        }

        public State Find(long id)
        {
            //Used for finding state by state id
            try
            {
                if (id != 0)
                {

                    return _context.State.AsNoTracking().Where(e => (e.Id) == id).FirstOrDefault();
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

        public JsonResponse AddUpdate(State State)
        {
            //Used for inserting and updating state
            //Stored Procedure Used for insert SP_SaveState
            //Stored Procedure Used for update SP_UpdateState
            // Createdby and Modifiedby parameters are hardcoded as of now 

            State.CreatedBy = 1;
            int data;
            if (State.Id == 0)
            {   if (!IsStateExists(State.StateName))
                {   try
                    {
                        SqlParameter[] ObjParams = new SqlParameter[] {
                         new SqlParameter("@StateName", State.StateName),
                         new SqlParameter("@CountryId", State.CountryId),
                         new SqlParameter("@IsUT",State.IsUnionTerritory),
                         new SqlParameter("@IsActive", State.IsActive),
                         new SqlParameter("@Statecode",State.StateCode),
                         new SqlParameter("@Gstcode",State.GSTStateCode),
                         new SqlParameter("@CreatedBy",GetUserID())
                        };
                        data = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.InsertState, ObjParams, CommandType.StoredProcedure);
                        if(data != 0)
                        { 
                        ObjParams = null;
                        resp.Status = Constants.ResponseStatus.Success;
                        resp.Message = Constants.Service.Data_insert_success;
                        }
                        else
                        {
                            resp.Status = Constants.ResponseStatus.Failed;
                            resp.Message = Constants.Service.Data_insert_failed;
                        }
                    }
                    catch (Exception ex)
                    {
                        resp.Status = Constants.ResponseStatus.Failed;
                        resp.Message = Constants.Service.Data_insert_failed;

                    }
                }   
            }
            else
            {

                try
                {
                    SqlParameter[] ObjParams = new SqlParameter[] {
                    new SqlParameter("@Id", State.Id),
                    new SqlParameter("@StateName", State.StateName),
                    new SqlParameter("@CountryId", State.CountryId),
                    new SqlParameter("@IsUT",State.IsUnionTerritory),
                    new SqlParameter("@IsActive", State.IsActive),
                    new SqlParameter("@Statecode",State.StateCode),
                    new SqlParameter("@Gstcode",State.GSTStateCode),
                    new SqlParameter("@ModifiedBy",GetUserID())
                        };
                    data = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateState, ObjParams, CommandType.StoredProcedure);
                    if (data != 0)
                    {
                        ObjParams = null;
                        resp.Status = Constants.ResponseStatus.Success;
                        resp.Message = Constants.Service.Data_Update_success;
                    }
                    else
                    {
                        resp.Status = Constants.ResponseStatus.Failed;
                        resp.Message = Constants.Service.Data_Update_failed;

                    }
                }
                catch (Exception ex)
                {
                    resp.Status = Constants.ResponseStatus.Failed;
                    resp.Message = Constants.Service.Data_Update_failed;
                    //throw ex;
                }
            }
            return resp;
        }

        public JsonResponse Deactivate(long ID, bool Status)
        {
            try
            {
                //If the ID was not null It goes to the change status part.
                if (ID != 0)
                {
                    var model = Find(ID);
                    if (model != null)
                    {
                        model.IsActive = Status;
                        model.ModifiedOn = DateTime.Now;
                        model.ModifiedBy = GetUserID();
                        _context.Set<State>().Update(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Status_changed_success;
                        }
                    }
                    else
                    {
                        resp.Status = Constants.ResponseStatus.Failed;
                        resp.Message = Constants.Service.Data_Update_failed;

                    }
                }
                // Else it Show the error message.
                else
                {
                    resp.Status = Constants.ResponseStatus.Failed;
                    resp.Message = Constants.Service.Data_Update_failed;
                }
            }
            catch(Exception ex)
            {
                resp.Status = Constants.ResponseStatus.Failed;
                resp.Message = Constants.Service.Data_Update_failed;
            }
            return resp;
        }
    }
}
