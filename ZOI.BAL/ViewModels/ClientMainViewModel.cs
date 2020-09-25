using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.ViewModels
{
    public class ClientMainViewModel : BaseViewModel
    {

        public ClientMainViewModel()
        {
            ClientDetails = new ClientMain();
            AddressDetails = new ClientAddresses();
            BankDetails = new ClientBankDetails();
            DepositryDetails = new ClientDepositoryDetails();
            EquityBrokerDetails = new ClientEquityBrokerDetails();
            PersonalDetails = new ClientPersonal();
            SecondaryContactDetails = new ClientSecondaryContact();
            MappingDetails = new ClientMapping();
          
        }

        //Client Models
        public ClientMain ClientDetails { get; set; }
        
        public ClientAddresses AddressDetails { get; set; }
        
        public ClientPersonal PersonalDetails { get; set; }
        
        public ClientBankDetails BankDetails { get; set; }
        
        public ClientEquityBrokerDetails EquityBrokerDetails { get; set; }
        
        public ClientSecondaryContact SecondaryContactDetails { get; set; }
        
        public ClientDepositoryDetails DepositryDetails { get; set; }

        public ClientMapping MappingDetails { get; set; }
        
        //Dropdown Listing main table

        public IEnumerable<SelectListItem> ClientFamilyList { get; set; }

        public IEnumerable<SelectListItem> TitleList { get; set; }

        public IEnumerable<SelectListItem> GenderList { get; set; }

        public IEnumerable<SelectListItem> OccupationList { get; set; }

        public IEnumerable<SelectListItem> EmployeeList { get; set; }

        //  Dropdowns for Addresses table
        public IEnumerable<SelectListItem> CountryList { get; set; }

        public IEnumerable<SelectListItem> StateList { get; set; }

        public IEnumerable<SelectListItem> CityList { get; set; }

        //  Dropdowns for Personal details

        public IEnumerable<SelectListItem> AnnualIncomeList { get; set; }

        public IEnumerable<SelectListItem> MaterialList { get; set; }

        public IEnumerable<SelectListItem> CitizenShipList { get; set; }

        public IEnumerable<SelectListItem> EducationList { get; set; }

        public IEnumerable<SelectListItem> ResidentalList { get; set; }

        public IEnumerable<SelectListItem> TradingExperienceList { get; set; }

        public IEnumerable<SelectListItem> CommodityTradeClassificationList { get; set; }


        //  Dropdowns for depository  details

        public IEnumerable<SelectListItem> MICRList { get; set; }

        public IEnumerable<SelectListItem> BankAccountTypeList { get; set; }
        public IEnumerable<SelectListItem> AccountTypeList { get; set; }

        //  Dropdowns for depository  details

        public IEnumerable<SelectListItem> DepositoryList { get; set; }


        //  Dropdowns for Equity Brokers details

        public IEnumerable<SelectListItem> BrokersList { get; set; }


        //  Dropdowns for Secondary Contact Details

        public IEnumerable<SelectListItem> RelationShipList { get; set; }

         //  Dropdowns for Client Mapping details

        public IEnumerable<SelectListItem>  SBUList{ get; set; }

        public IEnumerable<SelectListItem>  RMList{ get; set; }
        
        public IEnumerable<SelectListItem>  ModelList{ get; set; }
        
        public IEnumerable<SelectListItem>  BranchList{ get; set; }

        public int Flag { get; set; }


    }
}
