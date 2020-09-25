using System.Collections.Generic;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.BAL.Models.Base;

namespace ZOI.BAL.Services.Interface
{
    public interface IMenuService
    {
        JsonResponse Summary();
        
        public IEnumerable<Menu> ListAll();
        
        Menu GetData(long ID);
        
        public bool IsExsits(string name);
        
        public JsonResponse Deactivate(long ID, bool Status);
        
        public IEnumerable<SelectListItem> GetSubMenuList();
        
        public JsonResponse AddUpdate(Menu model);
        
        public IEnumerable<SelectListItem> GetAllMenuList();
        public IEnumerable<SelectListItem> GetGroupList();
    
    }
}
