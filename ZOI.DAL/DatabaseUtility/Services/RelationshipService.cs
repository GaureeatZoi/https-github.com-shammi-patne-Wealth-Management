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
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class RelationshipService: IRelationshipService
    {
        private readonly DatabaseContext _context;

        JsonResponse resp = new JsonResponse();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public RelationshipService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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


        //Add update Relationship Master
        public JsonResponse AddUpdate(Relationship model)
        {
            try
            {
                //  If model.ID == 0 the data goes to the Add part.   
                if (!IsExsits(model.RelationshipName, model.ID))
                {
                    if (model.ID == 0)
                    {
                        //Insert Relationship Table
                        model.CreatedOn = DateTime.Now;
                        model.CreatedBy = GetUserID();
                        _context.Set<Relationship>().Add(model);
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
                        var relationshipDetails = GetData(model.ID);

                        //Update Relationship Table
                        if (relationshipDetails != null)
                        {
                            relationshipDetails.RelationshipName = model.RelationshipName;
                            relationshipDetails.IsActive = model.IsActive;
                            relationshipDetails.ModifiedOn = DateTime.Now;
                            relationshipDetails.ModifiedBy = GetUserID();
                            _context.Set<Relationship>().Update(relationshipDetails);
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
                    resp.Message = "Relationship Name is already exists..";
                }
            }
            catch (Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
                throw ex;
            }
            return resp;

        }
        //Get Relationship Data from ID
        public Relationship GetData(int ID)
        {
            if (ID != 0)
            {
                try
                {
                    return _context.Relationship.AsNoTracking().Where(e => e.ID == ID).FirstOrDefault();
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //Check Relationship Name Exists or not
        public bool IsExsits(string name, int ID)
        {
            bool IsExsits = true;

            if (_context.Relationship.Where(e => e.RelationshipName == name && e.ID != ID).FirstOrDefault() == null)
            {
                IsExsits = false;
            }

            return IsExsits;
        }

        //Get Relationship Data for Listing
        public JsonResponse Summary()
        {
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetRelationShipData, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new Relationship
                {
                    ID = a.Field<int>("ID")
                   ,
                    RelationshipName = a.Field<string>("Relationship")
                   ,
                    IsActiveText = a.Field<string>("IsActive")
                   ,
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate")
                }).ToList();
            }
            catch(Exception ex)
            {
                resp.Message = Constants.Service.Common_message;
            }
            return resp;
        }

        //Change Relationship Status
        public JsonResponse Deactivate(int ID, bool Status)
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
                        _context.Set<Relationship>().Update(model);
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
