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
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.DAL;
using ZOI.BAL.DBContext;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ZOI.BAL.Services
{
    public class MenuService : IMenuService
    {
        private readonly DatabaseContext _context;

        JsonResponse resp = new JsonResponse();

        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenuService(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
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

        public Menu GetData(long ID)
        {  //Used for finding Menu by Id

            if (ID != 0)
            {
                try
                {
                    return _context.Menu.AsNoTracking().Where(e => (e.ID) == ID).FirstOrDefault();
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
        {  // For Unique Validation of Menu Name
            bool IsExsits = true;
            if (_context.Menu.Where(e => e.MenuName == name).FirstOrDefault() == null)
            {
                IsExsits = false;
            }
            return IsExsits;
        }

        public JsonResponse Deactivate(long ID, bool Status)
        {
            if (ID != 0)
            {
                resp.Message = "Data not deleted";
                try
                {
                    var model = GetData(ID);
                    model.IsActive = Status;
                    model.ModifiedOn = DateTime.Now;
                    model.ModifiedBy = GetUserID();
                    _context.Set<Menu>().Update(model);
                    int i = _context.SaveChanges();
                    if (i != 0)
                    {
                        resp.Status = "S";
                        resp.Message = "Data deleted successfully";
                    }
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
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetMenuList, null, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                resp.Data = data.Tables[1].AsEnumerable().Select(a => new Menu
                {
                    ID = a.Field<long>("ID"),
                    MenuName = a.Field<string>("MenuName"),
                    ParentMenuName = a.Field<string>("ParentMenuName"),
                    ControllerName = a.Field<string>("ControllerName"),
                    ActionName = a.Field<string>("ActionName"),
                    Params1 = a.Field<string>("Params1"),
                    Params2 = a.Field<string>("Params2"),
                    MenuIcon = a.Field<string>("MenuIcon"),
                    MenuOrder = a.Field<int>("MenuOrder"),
                    IsParentMenuText = a.Field<string>("IsParentMenuText"),
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


        public IEnumerable<Menu> ListAll()
        {
            // Used for Menu Listing
            //Stored Procedure Used for listing  Admin_GetMenu

            DataSet data = new DataSet();
            IEnumerable<Menu> MenuList;
            try
            {

                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetMenuList, null, CommandType.StoredProcedure);

                MenuList = data.Tables[1].AsEnumerable().Select(a => new Menu
                {
                    ID = a.Field<long>("ID"),
                    MenuName = a.Field<string>("MenuName"),
                    ParentMenuName = a.Field<string>("ParentMenuName"),
                    ControllerName = a.Field<string>("ControllerName"),
                    ActionName = a.Field<string>("ActionName"),
                    MenuIcon = a.Field<string>("MenuIcon"),
                    MenuOrder = a.Field<int>("MenuOrder"),
                    Params1 = a.Field<string>("Params1"),
                    Params2 = a.Field<string>("Params2"),
                    IsParentMenuText = a.Field<string>("IsParentMenuText"),
                    LastUpdatedDate = a.Field<string>("LastUpdatedDate"),
                    IsActiveText = a.Field<string>("Status")
                }).ToList();
            }
            catch
            {
                return null;
            }
            return MenuList;

        }

        public JsonResponse AddUpdate(Menu model)
        {   //Used for inserting and updating Menu
            //Stored Procedure Used for insert Admin_InsertMenu
            //Stored Procedure Used for update Admin_UpdateMenu
            // Createdby and Modifiedby parameters are hardcoded as of now 
            try
            {
                if (model.ID == 0)
                {
                    if (!IsExsits(model.MenuName))
                    {
                        SqlParameter[] ObjParams = new SqlParameter[]
                        {
                            new SqlParameter("@MenuName", model.MenuName),
                            new SqlParameter("@ParentMenuId", model.ParentMenuId),
                            new SqlParameter("@Controller", model.ControllerName),
                            new SqlParameter("@Action", model.ActionName),
                            new SqlParameter("@Params1", model.Params1),
                            new SqlParameter("@Params2", model.Params2),
                            new SqlParameter("@MenuOrder", model.MenuOrder),
                            new SqlParameter("@MenuIcon", model.MenuIcon),
                            new SqlParameter("@IsParentMenu",model.IsParentMenu),
                            new SqlParameter("@IsActive", model.IsActive),
                            new SqlParameter("@IsSubParentMenu", model.IsSubMenu),
                            new SqlParameter("@GroupID", model.GroupID),
                            new SqlParameter("@UserID",GetUserID())
                        };

                        int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.InsertMenu, ObjParams, CommandType.StoredProcedure);
                        if (i != 0)
                        {
                            resp.Status = "S";
                            resp.Message = "Data inserted successfully";
                        }
                        else
                        {
                            resp.Message = "Data inserted failed";
                        }
                    }
                    else
                    {
                        resp.Status = "F";
                        resp.Message = "This data already exists";
                    }
                }
                else
                {
                    resp.Message = "Data updated failed";

                    SqlParameter[] ObjParams = new SqlParameter[]
                    {
                        new SqlParameter("@ID ",model.ID),
                        new SqlParameter("@MenuName", model.MenuName),
                        new SqlParameter("@ParentMenuId", model.ParentMenuId),
                        new SqlParameter("@Controller", model.ControllerName),
                        new SqlParameter("@Action", model.ActionName),
                        new SqlParameter("@Params1", model.Params1),
                        new SqlParameter("@Params2", model.Params2),
                        new SqlParameter("@IsParentMenu",model.IsParentMenu),
                        new SqlParameter("@MenuOrder", model.MenuOrder),
                        new SqlParameter("@MenuIcon", model.MenuIcon),
                        new SqlParameter("@IsSubParentMenu", model.IsSubMenu),
                        new SqlParameter("@SubParentMenuID", model.SubParentMenuID),
                        new SqlParameter("@GroupID", model.GroupID),
                        new SqlParameter("@IsActive", model.IsActive),
                        new SqlParameter("@UserID",GetUserID())
                    };
                    int i = new ADODataFunction().ExecuteNonQuery(Constants.Procedures.UpdateMenu, ObjParams, CommandType.StoredProcedure);
                    if (i != 0)
                    {
                        resp.Status = "S";
                        resp.Message = "Data updated Successfully";
                    }
                    else
                    {
                        resp.Message = "Data updated failed";
                    }

                }
            }
            catch (Exception ex)
            {
                resp.Status = "F";
                resp.Message = "Data not Updated";
            }
            return resp;
        }

        public IEnumerable<SelectListItem> GetAllMenuList()
        { //Parent Menu Dropdown
            try
            {
                return _context.Menu.AsNoTracking().Where(e => e.IsActive && (e.IsParentMenu || e.IsSubMenu)).Select(e => new SelectListItem()
                {
                    Value = (e.ID).ToString(),
                    Text = e.MenuName
                }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<SelectListItem> GetGroupList()
        { //Parent Menu Dropdown
            try
            {
                return _context.MenuGroup.AsNoTracking().OrderBy(e => e.GroupName).Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.ID).ToString(),
                    Text = e.GroupName
                }).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<SelectListItem> GetSubMenuList()
        {
            try
            {
                return _context.Menu.AsNoTracking().OrderBy(e => e.MenuName).Where(e => e.IsActive == true && e.IsSubMenu == true).Select(e => new SelectListItem()
                {
                    Value = (e.ID).ToString(),
                    Text = e.MenuName
                }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<SelectListItem> GetMenuTypeList()
        {
            try
            {
                return _context.MenuType.AsNoTracking().OrderBy(e => e.MenuTypeName).Where(e => e.IsActive).Select(e => new SelectListItem()
                {
                    Value = (e.ID).ToString(),
                    Text = e.MenuTypeName
                }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}

