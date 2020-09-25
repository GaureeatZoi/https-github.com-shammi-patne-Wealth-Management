using System.Collections.Generic;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ZOI.BAL.Services.Interface
{
    public interface IDepositoryService
    {
        public  JsonResponse Summary();
        
        public IEnumerable<Depository> ListAll();
        
        Depository GetData(long ID);
        
        public bool IsExsits(string name, long ID);
        
        public JsonResponse Deactivate(long ID, bool Status);
        
        public JsonResponse AddUpdate(Depository model);
        
        public IEnumerable<SelectListItem> GetCountryList();
        
        public IEnumerable<SelectListItem> GetStateList(long? CountryId);
        
        public IEnumerable<SelectListItem> GetCityList(long? StateID);

    }
}
