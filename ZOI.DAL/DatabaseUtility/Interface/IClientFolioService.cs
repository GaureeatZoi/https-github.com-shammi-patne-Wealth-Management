using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.ViewModels;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IClientFolioService
    {
        JsonResponse AddUpdate(ClientFolioViewModel model);
        
        JsonResponse Summary();

        JsonResponse ChangeStatus(long ID, bool Status);

        ClientFolio GetClientFolio(long ID);
        
        //JsonResponse ChangeStatus(long ID);

        IEnumerable<SelectListItem> FillDropDown(string procedure,long? param);

        IEnumerable<SelectListItem> FillAccountDropDown(string procedure, long? param1, long? param2);

        ClientMain GetInvestorDetail(long ID);

        JsonResponse IsExsits(string FolioNo,long SchemaID);

    }
}
