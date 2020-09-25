using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IBankBranchService
    {
        public JsonResponse AddUpdate(BankBranch model);

        public JsonResponse Summary();

        public JsonResponse Deactivate(long ID, bool Status);

        public BankBranch GetData(long ID);

        public bool IsExsits(string name,long ID, long Bank);
        
        public bool IsIFSCExsits(string name);

        public bool IsMICRExsits(string code);

        public IEnumerable<SelectListItem> GetBankList();
               
        public IEnumerable<SelectListItem> GetCountryList();
               
        public IEnumerable<SelectListItem> GetStateList(long? CountryId);
               
        public IEnumerable<SelectListItem> GetCityList(long? StateID);

        public IEnumerable<SelectListItem> GetAddressesDropDowns(string Procedure, long? Parameter, string Flag);

    }
}
