using System.Collections.Generic;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.BAL;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IPincodeService
    {
        public JsonResponse Summary();
        public IEnumerable<PincodeMaster> ListAll();
        PincodeMaster GetData(long ID);
        public bool IsExsits(long name, long ID);
        public JsonResponse Deactivate(long ID, bool Status);
        public JsonResponse AddUpdate(PincodeMaster model);
        public IEnumerable<SelectListItem> GetCountryList();
        public IEnumerable<SelectListItem> GetStateList(long? CountryId);
        public IEnumerable<SelectListItem> GetCityList(long? StateID);
    }
}
