using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IRTATransactionTypeService
    {
        JsonResponse AddUpdate(RTATransactionTypes model);
        
        JsonResponse Summary();
       
        JsonResponse UnmappedSummary();

        JsonResponse MapData(List<TransactionTypeMappingList> model);

        bool IsExsits(long RTAID, string RTAType,long ID);

        RTATransactionTypes GetTransactionType(long ID);

        JsonResponse ChangeStatus(long ID, bool Status);
        
        IEnumerable<SelectListItem> FillDropDown(string Procedure);

    }
}
