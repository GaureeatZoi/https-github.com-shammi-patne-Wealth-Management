using System.Collections.Generic;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Services.Interface
{
    public interface IRoleService
    {
        JsonResponse Summary();
        public IEnumerable<Role> ListAll();
        Role GetData(int ID);
        public bool IsExsits(string name);
        public JsonResponse Deactivate(int ID, bool Status);
        public JsonResponse AddUpdate(Role model);
        public IEnumerable<SelectListItem> GetRoleList();
        public IEnumerable<SelectListItem> GetApplicationList();
    }
}
