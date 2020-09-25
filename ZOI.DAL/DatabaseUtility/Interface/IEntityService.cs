using System.Collections.Generic;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.BAL.ViewModels;
using ZOI.BAL;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IEntityService
    {
        JsonResponse Summary();
        public IEnumerable<Entity> ListAll();
        public EntityViewModel GetData(long ID);
        //  public string getEntityCode(int entitytype);
        public bool IsExsits(string name, long ID);
        public JsonResponse Deactivate(long ID, bool Status);
        public JsonResponse AddUpdate(EntityViewModel model);
        public IEnumerable<SelectListItem> GetCountryList();
        public IEnumerable<SelectListItem> GetStateList(long? CountryId);
        public IEnumerable<SelectListItem> GetCityList(long? StateID);
        public IEnumerable<SelectListItem> GetEntityTypeList();
        public IEnumerable<SelectListItem> GetManagerList();

    }
}
