using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.DAL;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ZOI.BAL.Services
{
    public class CityService : ICityService
    {
        //    private readonly DBContext.DatabaseContext databaseContext;
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonResponse resp = new JsonResponse();

        public CityService(DatabaseContext context
            , IHttpContextAccessor httpContextAccessor)
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


        public bool IsCityExists(string CityName)
        {
            //Used for Unique Cityname Validation
            bool IsExsits = true;
            //If the condition true the data with this name doestn't have the duplicate value in the database.
            if (_context.City.Where(e => e.CityName == CityName).FirstOrDefault() == null)
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
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetCityList, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new City
                {
                    Id = a.Field<long>("Id"),
                    CityName = a.Field<string>("CityName"),
                    CityTier = a.Field<string>("CityTier"),
                    StateId = a.Field<long>("StateId"),
                    StateName = a.Field<string>("StateName"),
                    CountryId = a.Field<long>("CountryId"),
                    CountryName = a.Field<string>("CountryName"),
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


        public IEnumerable<City> ListAll()
        {
            //Used for Unique City Listing
            //Stored procedure used SP_GetCity

            DataSet data = new DataSet();
            IEnumerable<City> CityList;
            try
            {
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetCityList, null, CommandType.StoredProcedure);
                CityList = data.Tables[1].AsEnumerable().Select(dataRow => new City
                {
                    Id = dataRow.Field<long>("Id"),
                    CityName = dataRow.Field<string>("CityName"),
                    CityTier = dataRow.Field<string>("CityTier"),
                    StateId = dataRow.Field<long>("StateId"),
                    StateName = dataRow.Field<string>("StateName"),
                    CountryId = dataRow.Field<long>("CountryId"),
                    CountryName = dataRow.Field<string>("CountryName"),
                    IsActiveText = dataRow.Field<string>("Status"),
                    LastUpdatedDate = dataRow.Field<string>("LastUpdatedDate")
                }).ToList();
                return CityList;
            }
            catch (Exception ex)
            {
                // StackTrace CallStack = new StackTrace(ex, true);
                //  ex.Data["ErrDescription"] = ex.Data["ErrDescription"] != null ? ex.Data["ErrDescription"] : string.Format("Error captured in {0} on Line No {1} of Method {2}", CallStack.GetFrame(0).GetFileName(), CallStack.GetFrame(0).GetFileLineNumber(), CallStack.GetFrame(0).GetMethod().ToString());
                // throw ex;
                return null;
            }

        }

        public IEnumerable<SelectListItem> InitCityView()
        {
            //Used for create  Statedropdown
           

            return _context.State.AsNoTracking().OrderBy(e => e.StateName).Where(e => e.IsActive == true ).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.StateName
            }).ToList();
            
        }

        public JsonResponse AddUpdate(City City)
        {    //Used for inserting and Updating City
            //Stored Procedure Used for insert SP_SaveCity
            //Stored Procedure Used for update SP_UpdateCity
            // Createdby and Modifiedby parameters are hardcoded as of now 
            int data;
            City.CreatedBy = GetUserID();
            if (City.Id == 0)
            {
                if (!IsCityExists(City.CityName))
                {
                    try
                    {
                        SqlParameter[] ObjParams = new SqlParameter[] {
                    new SqlParameter("@CityName", City.CityName),
                    new SqlParameter("@StateId", City.StateId),
                     new SqlParameter("@IsActive", City.IsActive),
                    new SqlParameter("@Tier", City.Tier),
                    new SqlParameter("@CreatedBy",City.CreatedBy)
                    };

                      
                            data = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.InsertCity, ObjParams, CommandType.StoredProcedure);
                        ObjParams = null;
                        if (data != 0)
                        {
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
                    City.ModifiedBy = GetUserID();
                    SqlParameter[] ObjParams = new SqlParameter[] {
                    new SqlParameter("@Id", City.Id),
                    new SqlParameter("@CityName", City.CityName),
                    new SqlParameter("@StateId", City.StateId),
                    new SqlParameter("@IsActive", City.IsActive),
                    new SqlParameter("@Tier", City.Tier),
                    new SqlParameter("@ModifiedBy",City.ModifiedBy)
                 };
                    data = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateCity, ObjParams, CommandType.StoredProcedure);
                    resp.Status = Constants.ResponseStatus.Success;
                    resp.Message = Constants.Service.Data_Update_success;

                }
                catch (Exception ex)
                {
                    resp.Status = "F";
                    resp.Message = "Data not deleted";
                    throw ex;
                }
            }
            return resp;
        }


        public City Find(long id)
        {   //Used for finding City by Id
            try
            {
                if (id != 0)
                {

                    return _context.City.AsNoTracking().Where(e => (e.Id) == id).FirstOrDefault();
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
                    _context.Set<City>().Update(model);
                    int i = _context.SaveChanges();
                    if (i != 0)
                    {
                        resp.Status = Constants.ResponseStatus.Success;
                        resp.Message = Constants.Service.Status_changed_success;
                    }
                    }
                    // Else it Show the error message.
                    else
                    {
                        resp.Message = Constants.Service.Common_message;
                    }
                }
                // Else it Show the error message.
                else
                {
                    resp.Status = Constants.ResponseStatus.Failed;
                    resp.Message = Constants.Service.Data_Update_failed;
                }
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        public IEnumerable<SelectListItem> GetCityTierList()
        { //CityTier Dropdown
            return _context.Enum.AsNoTracking().OrderBy(e => e.EnumCode).Where(e => e.IsActive == true && e.EnumType == "CityTier").Select(e => new SelectListItem()
            {
                Value = (e.EnumValue).ToString(),
                Text = e.EnumCode
            }).ToList();
        }
    }
}
