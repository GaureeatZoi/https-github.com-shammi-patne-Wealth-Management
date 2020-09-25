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

namespace ZOI.BAL.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly DatabaseContext _context;

        JsonResponse resp = new JsonResponse();        

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HolidayService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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
        {    //Used for Holiday listing
             //Stored Procedure Used for listing Admin_GetHoliday
            try
            { DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetHoliday, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new Holidays
                {
                    ID = a.Field<long>("Id"),
                    HolidayDate = a.Field<string>("HolidayDate"),
                    Holiday = a.Field<string>("Holiday"),
                    IsSettlementHday = a.Field<string>("IsSettlementHoliday"),
                    IsTradingHday = a.Field<string>("IsTradingHoliday"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch
            { resp.Message = "Data loaded failed";
            }
            return resp;
        }


        public IEnumerable<Holidays> ListAll()
        {
            //Used for Holiday listing
            //Stored Procedure Used for listing Admin_GetHoliday
            DataSet data = new DataSet();
            IEnumerable<Holidays> HolidayList;
            try
            {
                
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetHoliday, null, CommandType.StoredProcedure);

                HolidayList = data.Tables[1].AsEnumerable().Select(a => new Holidays
                {
                    ID = a.Field<long>("Id"),
                    HolidayDate = a.Field<string>("HolidayDate"),
                    Holiday = a.Field<string>("Holiday"),
                    IsSettlementHday = a.Field<string>("IsSettlementHoliday"),
                    IsTradingHday = a.Field<string>("IsTradingHoliday"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch
            {
                return null;
            }
            return HolidayList;

        }

        public Holidays GetData(long ID)
        { 
            if (ID != 0)
            { 
                try
                {  
                    return _context.Holiday.AsNoTracking().Where(e => (e.ID) == ID).FirstOrDefault();
                }
                catch
                {   return null;
                }
            }
            else
            {  return null;
            }
        }

        public bool IsExsits(string name, long ID)
        {  ////Used for changing unique validation of Holiday date
            bool IsExsits = true;
            if (_context.Holiday.Where(e => e.HolidayDate == name && e.ID != ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;
        }

            public JsonResponse Deactivate(long ID, bool Status)
            {   //Used for changing active deactive  status of Holiday
                if (ID != 0)
                {
                    resp.Message = "Something went wrong";
                    try
                    {
                        var model = GetData(ID);
                        model.IsActive = Status;
                        model.ModifiedOn = DateTime.Now;
                        model.ModifiedBy = 1;
                        _context.Set<Holidays>().Update(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = "S";
                            resp.Message = "Data deleted successfully";
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    resp.Message = "ID was not found";
                }
                return resp;
            }

        public JsonResponse AddUpdate(Holidays model)
        { //Used for inserting and updating Holiday
            //Stored Procedure Used for insert Admin_InsertHoliday
            //Stored Procedure Used for update Admin_UpdateHoliday
            // Createdby and Modifiedby parameters are hardcoded as of now 
            
            if (model.ID == 0)
            { if (!IsExsits(model.HolidayDate, model.ID))
                {  try
                    {  SqlParameter[] ObjParams = new SqlParameter[] {
                        
                         new SqlParameter("@HolidayDate", model.HolidayDate),
                         new SqlParameter("@Holiday", model.Holiday),
                         new SqlParameter("@IsSettlementHoliday", model.IsSettlementHoliday),
                         new SqlParameter("@IsTradingHoliday", model.IsTradingHoliday),
                         new SqlParameter("@IsActive", model.IsActive),
                         new SqlParameter("@CreatedBy",GetUserID())
                    };
                        new ADODataFunction().ExecuteNonQuery(Constants.Procedures.InsertHoliday, ObjParams, CommandType.StoredProcedure);
                        ObjParams = null;
                        resp.Status = "S";
                        resp.Message = "Data inserted successfully";
                    }
                    catch (Exception ex)
                    {  resp.Status = "F";
                        resp.Message = "Data not insert";
                        throw ex;
                    }
                }
                else
                { resp.Status = "F";
                    resp.Message = "This data already exsits";
                }
            }
            else
            {   resp.Message = "Data updated failed";
                try
                {     SqlParameter[] ObjParams = new SqlParameter[] {
                       new SqlParameter("@id ",model.ID),
                         new SqlParameter("@HolidayDate", model.HolidayDate),
                         new SqlParameter("@Holiday", model.Holiday),
                         new SqlParameter("@IsSettlementHoliday", model.IsSettlementHoliday),
                         new SqlParameter("@IsTradingHoliday", model.IsTradingHoliday),
                         new SqlParameter("@IsActive", model.IsActive),
                         new SqlParameter("@ModifiedBy",GetUserID())
                    };
                    new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateHoliday, ObjParams, CommandType.StoredProcedure);
                    ObjParams = null;
                    resp.Status = "S";
                    resp.Message = "Data updated Successfully";
                }
                catch (Exception ex)
                {   resp.Status = "F";
                    resp.Message = "Data not Updated";
                    throw ex;
                }
            }
            return resp;
        }
    }
}
