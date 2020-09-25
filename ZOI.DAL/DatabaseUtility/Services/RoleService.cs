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
    public class RoleService : IRoleService
    {
        private readonly DatabaseContext _context;

        JsonResponse resp = new JsonResponse();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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

        public Role GetData(int ID)
        {  //Used for finding Role by Id

            if (ID != 0)
            {
                try
                {
                    return _context.Role.AsNoTracking().Where(e => (e.RoleID) == ID).FirstOrDefault();
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

        public bool IsExsits(string name)
        {  // For Unique Validation of Role Name
            bool IsExsits = true;

            if (_context.Role.Where(e => e.Name == name).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;
        }

        public JsonResponse Deactivate(int ID, bool Status)
        {
            if (ID != 0)
            {
                resp.Message = "Data not deleted";
                try
                {
                    var model = GetData(ID);
                    model.IsActive = Status;
                    model.ModifiedBy = GetUserID();
                    //  _context.Set<Role>().Update(model);
                    //  int i = _context.SaveChanges();

                    SqlParameter[] ObjParams = new SqlParameter[] {
                         new SqlParameter("@RoleId ",model.RoleID),
                         new SqlParameter("@ID ",model.ID),
                         new SqlParameter("@RoleName", model.Name),
                         new SqlParameter("@RoleDescription", model.NormalizedName),
                         new SqlParameter("@ParentRoleId", model.ParentRoleId),
                         new SqlParameter("@ApplicationId", model.ApplicationId),
                         new SqlParameter("@IsActive", model.IsActive),
                         new SqlParameter("@ModifiedBy",GetUserID())
                  };

                    new ADODataFunction().ExecuteNonQuery("Admin_UpdateRole", ObjParams, CommandType.StoredProcedure);
                    ObjParams = null;
                    resp.Status = "S";
                    resp.Message = "Data updated Successfully";

                }
                catch (Exception ex)
                {
                    resp.Status = "F";
                    resp.Message = "Data not deleted";
                    throw ex;
                }
            }
            else
            {
                resp.Message = "ID was not found";
            }
            return resp;
        }

        public JsonResponse Summary()
        {   //Used for Menu Listing 
            //Stored Procedure Used for listing  Admin_GetMenu
            try
            {
                DataSet data = new DataSet();
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetRoleList, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new Role
                {
                    RoleID = a.Field<int>("RoleID"),
                    Name = a.Field<string>("RoleName"),
                    NormalizedName = a.Field<string>("RoleDescription"),
                    ParentRoleName = a.Field<string>("ParentRoleName"),
                    ApplicationName = a.Field<string>("ApplicationName"),
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

        public IEnumerable<Role> ListAll()
        {
            //Used for Menu Listing 
            //Stored Procedure Used for listing  Admin_GetMenu
            DataSet data = new DataSet();
            IEnumerable<Role> RoleList;
            try
            {
               
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetRoleList, null, CommandType.StoredProcedure);

                RoleList = data.Tables[1].AsEnumerable().Select(a => new Role
                {
                    RoleID = a.Field<int>("RoleID"),
                    Name = a.Field<string>("RoleName"),
                    NormalizedName = a.Field<string>("RoleDescription"),
                    ParentRoleName = a.Field<string>("ParentRoleName"),
                    ApplicationName = a.Field<string>("ApplicationName"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch
            {
               return null;
            }
            return RoleList;
        }
        public JsonResponse AddUpdate(Role model)
        {   //Used for inserting and updating Role
            //Stored Procedure Used for insert Admin_InsertRole
            //Stored Procedure Used for update Admin_UpdateMenu
            // Createdby and Modifiedby parameters are hardcoded as of now 
           
            if (model.RoleID == 0)
            {
                if (!IsExsits(model.Name))
                {
                    try
                    {
                        SqlParameter[] ObjParams = new SqlParameter[] {
                       
                         new SqlParameter("@ID ",model.ID),
                         new SqlParameter("@RoleName", model.Name),
                         new SqlParameter("@RoleDescription", model.NormalizedName),
                         new SqlParameter("@ParentRoleId", model.ParentRoleId),
                         new SqlParameter("@ApplicationId", model.ApplicationId),
                         new SqlParameter("@IsActive", model.IsActive),
                         new SqlParameter("@CreatedBy",GetUserID())
                    };

                        new ADODataFunction().ExecuteNonQuery(Constants.Procedures.InsertRole, ObjParams, CommandType.StoredProcedure);
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
                         new SqlParameter("@RoleId ",model.RoleID),
                         new SqlParameter("@ID ",model.ID),
                         new SqlParameter("@RoleName", model.Name),
                         new SqlParameter("@RoleDescription", model.NormalizedName),
                         new SqlParameter("@ParentRoleId", model.ParentRoleId),
                          new SqlParameter("@ApplicationId", model.ApplicationId),
                         new SqlParameter("@IsActive", model.IsActive),
                         new SqlParameter("@ModifiedBy",GetUserID())
                    };

                    new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateRole, ObjParams, CommandType.StoredProcedure);
                    ObjParams = null;
                    resp.Status = "S";
                    resp.Message = "Data updated Successfully";
                }
                catch (Exception ex)
                {
                    resp.Status = "F";
                    resp.Message = "Data not Updated";
                    throw ex;
                }
            }
            return resp;
        }

        public IEnumerable<SelectListItem> GetRoleList()
        { //Parent Role Dropdown
           // var list;
          return  _context.Role.AsNoTracking().OrderBy(e => e.Name).Where(e => e.IsActive == true).Select(e => new SelectListItem()
          {
              Value = (e.RoleID).ToString(),
              Text = e.Name
          }).ToList();

        }

        public IEnumerable<SelectListItem> GetApplicationList()
        { //Parent Role Dropdown
          // var list;
            return _context.Application.AsNoTracking().OrderBy(e => e.ApplicationName).Where(e => e.IsActive == true).Select(e => new SelectListItem()
            {
                Value = (e.ID).ToString(),
                Text = e.ApplicationName
            }).ToList();

        }


    }
}
