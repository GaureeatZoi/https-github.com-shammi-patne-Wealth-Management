using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ZOI.BAL;
using ZOI.BAL.DBContext;
using ZOI.BAL.Models;
using ZOI.BAL.Utilites;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public class BaseService : IBaseService
    {
        protected DatabaseContext _context;

        JsonResponse resp = new JsonResponse();

        public BaseService(DatabaseContext dBContextZoi)
        {
            _context = dBContextZoi;
        }

        public MenuPermission CurrentMenuPermission(string controller , int RoleID)
        {
            try
            {
                var menu = _context.Menu.Where(e => e.IsActive && e.ControllerName == controller).AsNoTracking().FirstOrDefault();               
                var permission = _context.RolePermission.Where(e => e.IsActive && e.MenuID == menu.ID && e.RoleID== RoleID).AsNoTracking().FirstOrDefault();
                return permission;
            }
            catch (Exception ex)
            {
                resp.Message = Constants.ResponseStatus.Failed;
                resp.Message = Constants.Service.Common_message;
                return null;
            }

        }

        public IEnumerable<CurrentUserMenu> FindUserMenus(int RoleID)
        {
            try
            {
                DataSet data = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@RoleID", RoleID);
                data = new ADODataFunction().ExecuteDataset(Constants.Procedures.GetCurrentUserMenu, param, CommandType.StoredProcedure);
                resp = data.Tables[0].AsEnumerable().Select(a => new JsonResponse
                {
                    Status = a.Field<string>("STATUS"),
                    Message = a.Field<string>("MESSAGE")
                }).FirstOrDefault();
                var menu = data.Tables[1].AsEnumerable().Select(a => new CurrentUserMenu
                {
                    ID=a.Field<long>("ID")
                    ,
                    MenuName = a.Field<string>("MenuName")
                   ,
                    MenuIcon = a.Field<string>("MenuIcon")
                    ,
                    MenuOrder = a.Field<int>("MenuOrder")
                    ,
                    ParentMenuId = a.Field<long>("ParentMenuId")
                    ,
                    IsParentMenu = Convert.ToBoolean(a.Field<bool>("IsParentMenu"))
                    ,
                    ControllerName = a.Field<string>("ControllerName")
                    ,
                    ActionName = a.Field<string>("ActionName")
                    ,
                    Params1 = a.Field<string>("Params1")
                    ,
                    Params2 = a.Field<string>("Params2")
                    ,
                    GroupID = a.Field<long>("GroupID")
                    ,
                    IsSubMenu = a.Field<bool>("IsSubParentMenu")
                    ,
                    SubParentMenuID = a.Field<long>("SubParentMenuID")
                    ,
                    ExportCSV = Convert.ToBoolean(a.Field<bool>("ExportCSV"))
                    ,
                    ExportExcel = Convert.ToBoolean(a.Field<bool>("ExportExcel"))
                    ,
                    ExportPDF = Convert.ToBoolean(a.Field<bool>("ExportPDF"))
                    ,
                    ReadAccess = Convert.ToBoolean(a.Field<bool>("ReadAccess"))
                    ,
                    WriteAccess = Convert.ToBoolean(a.Field<bool>("WriteAccess"))
                    ,
                    SettingsAccess = Convert.ToBoolean(a.Field<bool>("SettingsAccess"))
                }).ToList();
                return menu;
            }
            catch (Exception ex)
            {
                resp.Message = Constants.ResponseStatus.Failed;
                resp.Message = Constants.Service.Common_message;
                return null;
            }
        }

        public IEnumerable<MenuPermission> UserAccessableMenus(int RoleID)
        {
            try
            {
                List<MenuPermission> activeMenus = _context.RolePermission.AsQueryable().Where(e => e.IsActive && e.Read && e.RoleID == @RoleID).ToList();
                return activeMenus;
            }
            catch (Exception ex)
            {
                resp.Message = Constants.ResponseStatus.Failed;
                resp.Message = Constants.Service.Common_message;
                return null;
            }
        }


        //public string role(string username)
        //{
        //    var user = _context.Users.where(e => e.employeeid == username).firstordefault();
        //    var userrole = convert.toint32(user.roleid);
        //    var roles = _context.roles.where(e => e.id == userrole).firstordefault();
        //    var role = roles.rolename;
        //    return role;
        //}

        //public string ReportingManager(string username)
        //{
        //    var user = _context.Users.Where(e => e.EmployeeID == username).FirstOrDefault();
        //    if (user.ReportingManagerID != 0)
        //    {
        //        var managers = _context.Users.Where(e => e.ID == user.ReportingManagerID).FirstOrDefault();
        //        var manager = managers.DisplayName;
        //        return manager;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        //public User Notification(string username)
        //{
        //    var user = _context.Users.Where(e => e.EmployeeID == username).FirstOrDefault();
        //    return user;
        //}

        //public IEnumerable<Menu> FindUserMenus(int RoleID)
        //{
        //    //dBContext.UserSession = new DummyUserSession() { User = username };
        //    //var activeMenus = _context.RolePermission.AsQueryable().Where(e => e.IsActive && e.Read && e.RoleID == RoleID);
        //    //return activeMenus.Select(e => e.MenuID).Where(e => e.IsActive).Distinct().ToList().OrderBy(e => e.Order);


        //    return ;
        //}

        //private class DummyUserSession : IUserSession
        //{
        //    public string User { get; set; }
        //}

        //private User __CurrentUser = null;
        //public User CurrentLoggedInUser 
        //{
        //    get
        //    {
        //        if (__CurrentUser == null && _context.UserSession != null)
        //        {
        //            __CurrentUser = _context.Users.AsQueryable().Where(e => e.EmployeeID == _context.UserSession.User && e.IsActive).FirstOrDefault();
        //        }

        //        return __CurrentUser;
        //    }
        //}

        //public bool GetMenuEditPermission(string controller, string action, int flag, string userID)
        //{
        //    bool IsAllowed = false;
        //    SqlParameter[] sqlparams = new SqlParameter[] {
        //                     new SqlParameter("@UserID",userID),
        //                     new SqlParameter("@Controller",controller),
        //                     new SqlParameter("@Action",action),
        //                     new SqlParameter("@Flag",flag)
        //                     };
        //    DataTable dataTable = CommonMethods.ExecuteDatatable(Constants.Procedures.CheckAccessPermission, sqlparams);
        //    if (!CommonMethods.IsDatatableEmpty(dataTable) && Convert.ToBoolean(dataTable.Rows[0]["IsAllowed"]))
        //    {
        //        IsAllowed = Convert.ToBoolean(dataTable.Rows[0]["IsAllowed"]);
        //    }
        //    return IsAllowed;
        //}

    }
}
