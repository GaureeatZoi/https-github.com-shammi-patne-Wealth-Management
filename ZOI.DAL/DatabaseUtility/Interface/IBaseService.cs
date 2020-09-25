using System.Collections.Generic;
using ZOI.BAL.Models;
using ZOI.BAL.ViewModels;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IBaseService
    {
        IEnumerable<CurrentUserMenu> FindUserMenus(int RoleID);

        IEnumerable<MenuPermission> UserAccessableMenus(int RoleID);
        
        MenuPermission CurrentMenuPermission(string controller , int RoleID);

        //public string Role(string username);

        //public string ReportingManager(string username);

    }
}
