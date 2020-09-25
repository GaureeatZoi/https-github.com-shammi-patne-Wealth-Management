using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IRolePermissionService
    {

        public JsonResponse AddUpdate(List<MenuPermissionList> model);

        public JsonResponse Summary(int RoleID);

        public IEnumerable<SelectListItem> GetRolesData();



    }
}
