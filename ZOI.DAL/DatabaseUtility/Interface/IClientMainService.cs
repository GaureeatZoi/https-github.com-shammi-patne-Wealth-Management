using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.ViewModels;
using ZOI.DAL.DatabaseUtility.Services;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IClientMainService
    {
        JsonResponse AddUpdate(ClientMainViewModel model);
        
        public JsonResponse Summary();

        public JsonResponse Deactivate(long ID, bool Status);

        ClientMain GetData(long ID);


        //ClientMainViewModel GetDatas(long ID);
        
        ClientAddresses GetAddressData(long ID);

        ClientMapping GetMappingDetailData(long ID);

        ClientPersonal GetPersonalDetailData(long ID);

        ClientBankDetails GetBankDetailsData(long ID);

        ClientEquityBrokerDetails GetEquityBrokersData(long ID);

        ClientSecondaryContact GetSecondaryContactDetailData(long ID);

        ClientDepositoryDetails GetDepositryDetailData(long ID);

        JsonResponse GetAddressesDropDowns(string Procedure, long Parameter, string Flag);

        IEnumerable<SelectListItem> GetDropDownList(string Procedure);

        IEnumerable<SelectListItem> FillEnumList(string EnumType);

        bool IsExsits(string Flag,string Data);


    }
}
