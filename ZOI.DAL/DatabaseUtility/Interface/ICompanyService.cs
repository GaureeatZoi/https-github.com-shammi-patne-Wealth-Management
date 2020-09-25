using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface ICompanyService
    {
        JsonResponse AddUpdate(CompanyMaster model);
        bool IsExsits(string name, long ID);
        JsonResponse Deactivate(long ID, bool Status);
        CompanyMaster GetData(long ID);
        JsonResponse Summary();
        IEnumerable<SelectListItem> GetCountryList();
        IEnumerable<SelectListItem> GetStateList(long? CountryId);
        IEnumerable<SelectListItem> GetCityList(long? StateID);
        JsonResponse CheckImage(CompanyMaster model);
        long GetCompanyCode();
    }
}
