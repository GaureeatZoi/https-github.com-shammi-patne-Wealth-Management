using System.Collections.Generic;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ZOI.BAL.Services.Interface
{
    public interface ICountryService
    {
        public JsonResponse Summary();
        IEnumerable<Country> ListAll();
        Country Find(long? id);
        public JsonResponse Deactivate(long ID, bool Status);
        bool IsCountryExists(string CountryName);
        public JsonResponse AddUpdate(Country Country);
        public IEnumerable<SelectListItem> GetTimeZoneList();
        public long GetUserID();
    }
}
