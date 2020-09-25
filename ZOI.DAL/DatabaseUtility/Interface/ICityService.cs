using System.Collections.Generic;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ZOI.BAL.Services.Interface
{
    public interface ICityService
    {
        IEnumerable<City> ListAll();
        City Find(long id);
        public JsonResponse Deactivate(long ID, bool Status);
        public IEnumerable<SelectListItem> InitCityView();
        bool IsCityExists(string CityName);
        public JsonResponse AddUpdate(City model);
        public IEnumerable<SelectListItem> GetCityTierList();
        public JsonResponse Summary();
    }
}
