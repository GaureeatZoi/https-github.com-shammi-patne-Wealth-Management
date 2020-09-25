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
    public class CountryService : ICountryService
    {
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonResponse resp = new JsonResponse();

        public CountryService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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

        public bool IsCountryExists(string CountryName)
        {
            bool IsExsits = true;
            //If the condition true the data with this name doestn't have the duplicate value in the database.
            if (_context.Country.Where(e => e.CountryName == CountryName ).FirstOrDefault() == null)
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
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetCountryList, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new Country
                {
                    Id = a.Field<long>("Id"),
                    CountryName = a.Field<string>("CountryName"),
                    Currency = a.Field<string>("Currency"),
                    CurrencySymbolUnicode = a.Field<string>("CurrencySymbolUnicode"),
                    TimeZoneName = a.Field<string>("TimeZone"),
                    CountryCode = a.Field<int>("CountryCode"),
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




        public IEnumerable<Country> ListAll()
        {
            // Used for getting Country list
            // Sp used is SP_GetCountry

            DataSet data = new DataSet();
            IEnumerable<Country> CountryList;
            try
            {   data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetCountryList, null, CommandType.StoredProcedure);
                CountryList = data.Tables[1].AsEnumerable().Select(dataRow => new Country
                {   Id = dataRow.Field<long>("Id"),
                    CountryName = dataRow.Field<string>("CountryName"),
                    Currency = dataRow.Field<string>("Currency"),
                    CurrencySymbolUnicode = dataRow.Field<string>("CurrencySymbolUnicode"),
                    TimeZoneName = dataRow.Field<string>("TimeZone"),
                    IsActiveText = dataRow.Field<string>("Status"),
                   LastUpdatedDate = dataRow.Field<string>("LastUpdatedDate")
                }).ToList();
                return CountryList;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        public Country Find(long? id)
        {    // Used for finding Country by Id
             

            try
            {
                if (id != 0)
                {

                    return _context.Country.AsNoTracking().Where(e => (e.Id) == id).FirstOrDefault();
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
                        _context.Set<Country>().Update(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Status_changed_success;
                        }
                    }
                    else
                    {
                        resp.Message = Constants.Service.Common_message;
                    }
                }
                // Else it Show the error message.
                else
                {
                    resp.Message = Constants.Service.Common_message;
                }
            }
            catch(Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        public JsonResponse AddUpdate(Country Country)
        {   //Used for inserting and updating state
            //Stored Procedure Used for insert SP_SaveState
            //Stored Procedure Used for update SP_UpdateState
            // Createdby and Modifiedby parameters are hardcoded as of now 
            int data;

            if (Country.Id == 0)
            {if (!IsCountryExists(Country.CountryName))
               {try
                  { SqlParameter[] ObjParams = new SqlParameter[] {
                    new SqlParameter("@CountryName", Country.CountryName),
                    new SqlParameter("@Currency", Country.Currency),
                    new SqlParameter("@CurrencySymbol", Country.CurrencySymbolUnicode),
                    new SqlParameter("@TimeZone", Country.TimeZone),
                    new SqlParameter("@CountryCode", Country.CountryCode),
                     new SqlParameter("@IsActive", Country.IsActive),
                        new SqlParameter("@CreatedBy", GetUserID())
                        };
                        data = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.InsertCountry, ObjParams, CommandType.StoredProcedure);
                        if (data != 0)
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

                         return null;
                    }
                }
            }
            else
            {    try
                    { SqlParameter[] ObjParams = new SqlParameter[] {
                    new SqlParameter("@Id", Country.Id),
                    new SqlParameter("@CountryName", Country.CountryName),
                    new SqlParameter("@Currency", Country.Currency),
                    new SqlParameter("@CurrencySymbol", Country.CurrencySymbolUnicode),
                    new SqlParameter("@TimeZone", Country.TimeZone),
                      new SqlParameter("@CountryCode", Country.CountryCode),
                      new SqlParameter("@IsActive", Country.IsActive),
                        new SqlParameter("@ModifiedBy",GetUserID())
                        };
                        data = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateCountry, ObjParams, CommandType.StoredProcedure);

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

                }
            }
            return resp;
        }

        public IEnumerable<SelectListItem> GetTimeZoneList()
        { //timezone Dropdown
            return _context.TimeZone.AsNoTracking().OrderBy(e => e.TimeZoneName).Where(e => e.IsActive == true ).Select(e => new SelectListItem()
            {
                Value = (e.ID).ToString(),
                Text = e.TimeZoneName
            }).ToList();
        }

    }
}
