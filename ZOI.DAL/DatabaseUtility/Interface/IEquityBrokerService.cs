using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IEquityBrokerService
    {
        public JsonResponse AddUpdate(EquityBrokers model);

        public JsonResponse Summary();

        public JsonResponse Deactivate(long ID, bool Status);

        public EquityBrokers GetData(long ID);

        public bool IsExsits(string name,string Flag,long ID);

        IEnumerable<SelectListItem> GetCompanyList();

        public IEnumerable<SelectListItem> GetCountryList();
               
        public IEnumerable<SelectListItem> GetStateList(long? CountryId);
               
        public IEnumerable<SelectListItem> GetCityList(long? StateID);

        //JsonResponse UploadLogo(EquityBrokers modal);

        JsonResponse CheckImage(EquityBrokers modal);

    }
}
