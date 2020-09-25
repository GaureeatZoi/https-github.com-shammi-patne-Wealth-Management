using System.Collections.Generic;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ZOI.BAL.Services.Interface
{
    public interface IDepositoryService
    {
        public  JsonResponse Summary();
        public IEnumerable<Dipository> ListAll();
        Dipository GetData(long ID);
        public bool IsExsits(string name, long ID);
        public JsonResponse Deactivate(long ID, bool Status);
        public JsonResponse AddUpdate(Dipository model);
        public IEnumerable<SelectListItem> GetCountryList();
        public IEnumerable<SelectListItem> GetStateList(long? CountryId);
        public IEnumerable<SelectListItem> GetCityList(long? StateID);

    }
}
