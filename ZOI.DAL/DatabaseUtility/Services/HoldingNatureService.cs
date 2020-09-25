using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Utilites;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class HoldingNatureService: IHoldingNatureService
    {
        private readonly DatabaseContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonResponse resp = new JsonResponse();
        public HoldingNatureService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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

        //Add update Holding Master
        public JsonResponse AddUpdate(HoldingNature model)
        {
            try
            {
                //  If model.ID == 0 the data goes to the Add part.   
                if (!IsExsits(model.HNCode, model.ID))
                {
                    if (model.ID == 0)
                    {
                        //Insert HoldingNature Table
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        _context.Set<HoldingNature>().Add(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Data_insert_success;
                        }
                        else
                        {
                            resp.Status = Constants.ResponseStatus.Failed;
                        }

                    }
                    //  Else data goes to the Update part.
                    else
                    {
                        resp.Message = Constants.Service.Data_Update_failed;
                        var holdingNatureDetails = GetData(model.ID);

                        //Update holdingNature Table
                        if (holdingNatureDetails != null)
                        {
                            holdingNatureDetails.HNCode = model.HNCode;
                            holdingNatureDetails.HoldingType = model.HoldingType;
                            holdingNatureDetails.IsActive = model.IsActive;
                            holdingNatureDetails.ModifiedOn = DateTime.Now;
                            holdingNatureDetails.ModifiedBy = GetUserID();
                            _context.Set<HoldingNature>().Update(holdingNatureDetails);
                            int i = _context.SaveChanges();
                            if (i != 0)
                            {
                                resp.Status = Constants.ResponseStatus.Success;
                                resp.Message = Constants.Service.Data_Update_success;
                            }
                        }
                    }
                }
                else
                {
                    resp.Status = Constants.ResponseStatus.Failed;
                    resp.Message = "HNCode Name is already exists..";
                }
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
                throw ex;
            }
            return resp;

        }

        //Get HoldingNature Data from ID
        public HoldingNature GetData(long ID)
        {
            if (ID != 0)
            {
                try
                {
                    return _context.HoldingNature.AsNoTracking().Where(e => e.ID == ID).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //Check HoldingNature HnCode Exists or not
        public bool IsExsits(string hnCode, long ID)
        {
            bool IsExsits = true;

            if (_context.HoldingNature.Where(e => e.HNCode == hnCode && e.ID != ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }

            return IsExsits;
        }

        //Get HoldingNature Data for Listing
        public JsonResponse Summary()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset("Admin_GetHoldingNatureMasterData", null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new HoldingNature
                {
                    ID = a.Field<long>("ID")
                   ,HNCode = a.Field<string>("HNCode")
                   , HoldingType = a.Field<string>("HoldingType")
                   ,IsActiveText = a.Field<string>("IsActive")
                   ,LastUpdatedDate = a.Field<string>("LastUpdatedDate")
                }).ToList();
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        //Change HoldingNature Master Status
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
                        _context.Set<HoldingNature>().Update(model);
                        int i = _context.SaveChanges();
                        if (i != 0)
                        {
                            resp.Status = Constants.ResponseStatus.Success;
                            resp.Message = Constants.Service.Status_changed_success;
                        }
                    }
                }
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
