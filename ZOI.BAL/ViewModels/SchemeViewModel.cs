using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class SchemeViewModel:BaseViewModel
    {
        public SchemeViewModel()
        {
            this.schemeMaster = new Schema();
            this.Frequency = new Frequency();
            this.schemeRegistrationDetails = new SchemeRegistration();
            this.SchemeTransactionDetails = new SchemeTransaction();
            this.schemeRestrictedNationality = new SchemeRestrictedNationality();
            this.schemeRestrictedInvestorType = new SchemeRestrictedInvestorType();
        }
        public Schema schemeMaster { get; set; }
        public SchemeTransaction SchemeTransactionDetails { get; set; }
        public SchemeRegistration schemeRegistrationDetails { get; set; }
        public Frequency Frequency { get; set; }
        public SchemeRestrictedNationality schemeRestrictedNationality { get; set; }
        public SchemeRestrictedInvestorType schemeRestrictedInvestorType { get; set; }


        public IEnumerable<SelectListItem> RtaList { get; set; }
        public IEnumerable<SelectListItem> SchemeTypeList { get; set; }
        public IEnumerable<SelectListItem> AmcList { get; set; }
        public IEnumerable<SelectListItem> PlanList { get; set; }
        public IEnumerable<SelectListItem> OptionTypeList { get; set; }
        public IEnumerable<SelectListItem> RestrictNationalityList { get; set; }
        public IEnumerable<SelectListItem> InvestorTypeList { get; set; }
        public IEnumerable<SelectListItem> NoOfDaysList { get; set; }
        public IEnumerable<SelectListItem> NoOfMonthsList { get; set; }
        public IEnumerable<SelectListItem> NoOfYearsList { get; set; }
        public IEnumerable<SelectListItem> SeriesList { get; set; }
        public IEnumerable<SelectListItem> AssetClassList { get; set; }
        public IEnumerable<SelectListItem> SubAssetClassList { get; set; }

    }
}
