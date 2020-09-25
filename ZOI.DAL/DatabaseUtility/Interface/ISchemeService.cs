using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.ViewModels;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface ISchemeService
    {
        IEnumerable<SelectListItem> GetRTAList();
        IEnumerable<SelectListItem> GetAmcList();
        List<SelectListItem> GetSchemeTypeList();
        List<SelectListItem> GetPlanList();
        List<SelectListItem> GetOPtionTypeList();
        JsonResponse AddUpdate(SchemeViewModel model);
        bool IsExsits(string name, long ID);
        IEnumerable<SchemeViewModel> Summary();
        Schema GetData(long ID);
        JsonResponse Deactivate(long ID, bool Status);
        IEnumerable<SelectListItem> GetCountryList();
        List<SelectListItem> GetSeriesList();
        List<SelectListItem> GetAssetClassList();
        List<SelectListItem> GetMinimumHoldingPeriodDaysList();
        List<SelectListItem> GetMinimumHoldingPeriodMonthsList();
        List<SelectListItem> GetMinimumHoldingPeriodYearsList();
        SchemeTransaction GetTransactionData(long SchemeID);
        SchemeRegistration GetRegistrationData(long SchemeID);
        Frequency GetFrequncyData(long ID);
        SchemeRestrictedInvestorType GetRestrictedInvestorData(long ID);
        SchemeRestrictedNationality GetRestrictedNationality(long ID);
        List<string> GetSchemesRestrictedNationality(long ID);
        List<string> GetSchemesRestrictedInvestorType(long ID);

    }
}
