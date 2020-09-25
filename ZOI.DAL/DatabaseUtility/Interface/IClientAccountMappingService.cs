using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IClientAccountMappingService
    {

        JsonResponse AddUpdate(ClientAccountsMapping model);

        ClientAccountsMapping GetDataByID(long ID);

        JsonResponse Summary();

        JsonResponse IsExists(long ClientID, int AccountTypeID, string UCC);
        
        JsonResponse ChangeStatus(long ID, bool Status);

        List<ClientAccountsMapping> GetClientAccountsData(long ClientID);
        
        IEnumerable<SelectListItem> AccountTypeList();

        IEnumerable<SelectListItem> ClientList();

        IEnumerable<SelectListItem> EmployeeList();

    }
}
