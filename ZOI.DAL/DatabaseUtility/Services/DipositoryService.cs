using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Services.Interface;
using ZOI.BAL.Utilites;
using ZOI.DAL;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ZOI.BAL.Services
{
    public class DipositoryService : IDipositoryService
    {
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonResponse resp = new JsonResponse();

        public DipositoryService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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

        public JsonResponse Summary()
        {   //Used for Diository Listing 
            //Stored Procedure Used for listing  Admin_GetDipository
          try  {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetDipositorySummary, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new Dipository
                {
                    Id = a.Field<long>("Id"),
                    DPCode = a.Field<string>("DPCode")  ,
                    DPName = a.Field<string>("DPName")  ,
                    AddressLine1 = a.Field<string>("Address") ,
                    StateName = a.Field<string>("State")  ,
                    CityName = a.Field<string>("City")  ,
                    CountryName = a.Field<string>("Country"),
                    Pincode = a.Field<long>("Pincode"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate")  ,
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch
            {
                 resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }


        public IEnumerable<Dipository> ListAll()
        {
            //Used for Diository Listing 
            //Stored Procedure Used for listing  Admin_GetDipository
            IEnumerable<Dipository> DipositoryList;
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetDipositorySummary, null, CommandType.StoredProcedure);
              
                DipositoryList = data.Tables[1].AsEnumerable().Select(a => new Dipository
                {
                    Id = a.Field<long>("Id"),
                    DPCode = a.Field<string>("DPCode"),
                    DPName = a.Field<string>("DPName"),
                    AddressLine1 = a.Field<string>("Address"),
                    StateName = a.Field<string>("State"),
                    CityName = a.Field<string>("City"),
                    CountryName = a.Field<string>("Country"),
                    Pincode = a.Field<long>("Pincode"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch
            {
                return null;
            }

            return DipositoryList;
        }


        public Dipository GetData(long ID)
        {  //Used for finding Diository by Id

            if (ID != 0)
            {   try
                {  return _context.Dipository.AsNoTracking().Where(e => (e.Id) == ID).FirstOrDefault();
                }
                catch
                {  return null;
                }
            }
            else
            {  return null;
            }
        }

        public bool IsExsits(string name, long ID)
        {  // For Unique Validation of depositoryName
            bool IsExsits = true;
            if (_context.Dipository.Where(e => e.DPName == name && e.Id != ID).FirstOrDefault() == null)
            {  IsExsits = false;
            }
            return IsExsits;
        }

        public JsonResponse Deactivate(long ID, bool Status)
        { if (ID != 0)
            {  resp.Message = "Data not deleted";
                try
                { var model = GetData(ID);
                    model.IsActive = Status;
                    model.ModifiedOn = DateTime.Now;
                    model.ModifiedBy = GetUserID();
                    _context.Set<Dipository>().Update(model);
                    int i = _context.SaveChanges();
                    if (i != 0)
                    {   resp.Status = "S";
                        resp.Message = "Data deleted successfully";
                    }
                }
                catch (Exception ex)
                {   resp.Status = "F";
                    resp.Message = "Data not deleted";
                    throw ex;
                }
            }
            else
            { resp.Message = "ID was not found";
            }
            return resp;
        }

        public JsonResponse AddUpdate(Dipository model)
        {   //Used for inserting and updating state
            //Stored Procedure Used for insert Admin_InsertDipository
            //Stored Procedure Used for update Admin_UpdateDipository
            // Createdby and Modifiedby parameters are hardcoded as of now            
            if (model.Id == 0)
            {
                if (!IsExsits(model.DPCode, model.Id))
                {
                    try
                    {
                        SqlParameter[] ObjParams = new SqlParameter[] {
                        new SqlParameter("@id ",model.Id),
                         new SqlParameter("@DPCode", model.DPCode),
                         new SqlParameter("@DPName", model.DPName),
                         new SqlParameter("@AddressLine1", model.AddressLine1),
                         new SqlParameter("@AddressLine2", model.AddressLine2),
                         new SqlParameter("@Pincode", model.Pincode),
                         new SqlParameter("@CityId", model.CityId),
                         new SqlParameter("@StateId",model.StateId),
                         new SqlParameter("@CountryId", model.CountryId),
                         new SqlParameter("@Longitude", model.Longitude),
                         new SqlParameter("@Latitude", model.Latitude),
                         new SqlParameter("@IsActive", model.IsActive),
                         new SqlParameter("@CreatedBy",GetUserID())
                    };

                        new ADODataFunction().ExecuteNonQuery("Admin_InsertDipository", ObjParams, CommandType.StoredProcedure);
                        ObjParams = null;
                        resp.Status = "S";
                        resp.Message = "Data inserted successfully";

                    }
                    catch (Exception ex)
                    {
                        resp.Status = "F";
                        resp.Message = "Data not insert";
                        throw ex;
                    }
                }
                else
                {
                    resp.Status = "F";
                    resp.Message = "This data already exsits";
                }
            }
            else
            {
                resp.Message = "Data updated failed";
                try
                {
                    SqlParameter[] ObjParams = new SqlParameter[] {
                        new SqlParameter("@id ",model.Id),
                         new SqlParameter("@DPCode", model.DPCode),
                         new SqlParameter("@DPName", model.DPName),
                         new SqlParameter("@AddressLine1", model.AddressLine1),
                         new SqlParameter("@AddressLine2", model.AddressLine2),
                          new SqlParameter("@Pincode", model.Pincode),
                         new SqlParameter("@CityId", model.CityId),
                         new SqlParameter("@StateId",model.StateId),
                         new SqlParameter("@CountryId", model.CountryId),
                         new SqlParameter("@Longitude", model.Longitude),
                         new SqlParameter("@Latitude", model.Latitude),
                         new SqlParameter("@IsActive", model.IsActive),
                         new SqlParameter("@ModifiedBy",GetUserID())
                    };

                    new ADODataFunction().ExecuteNonQuery("Admin_UpdateDipository", ObjParams, CommandType.StoredProcedure);
                    ObjParams = null;
                        resp.Status = "S";
                        resp.Message = "Data updated Successfully";
                }
                catch(Exception ex)
                {
                    resp.Status = "F";
                    resp.Message = "Data not Updated";
                    throw ex;
                }
            }
            return resp;
        }

        public IEnumerable<SelectListItem> GetCountryList()
        { //Country Dropdown
            return _context.Country.AsNoTracking().OrderBy(e => e.CountryName).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CountryName
            }).ToList();
        }
        public IEnumerable<SelectListItem> GetStateList(long? CountryId)
        { //State Dropdown

            if (CountryId == null)
            {
                var d = _context.State.AsNoTracking().OrderBy(e => e.StateName).Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.Id).ToString(),
                    Text = e.StateName
                }).ToList();
                return d;
            }
            return _context.State.AsNoTracking().OrderBy(e => e.StateName).Where(e => e.CountryId == CountryId && e.IsActive).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.StateName
            }).ToList();

        }
        public IEnumerable<SelectListItem> GetCityList(long? StateID)
        {//City Dropdown
            if (StateID == null)
            {
                return _context.City.AsNoTracking().OrderBy(e => e.CityName).Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.Id).ToString(),
                    Text = e.CityName
                }).ToList();
            }
            return _context.City.AsNoTracking().OrderBy(e => e.CityName).Where(e => e.IsActive && e.StateId == StateID).Select(e => new SelectListItem()
            {
                Value = (e.Id).ToString(),
                Text = e.CityName
            }).ToList();
           
        }
    }
}
