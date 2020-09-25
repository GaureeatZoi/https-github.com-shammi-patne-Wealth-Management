using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;
using ZOI.BAL.ViewModels;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface ITransactionTypeService
    {
        JsonResponse AddUpdate(TransactionTypeViewModel model);
        
        JsonResponse Summary();

        TransactionTypes GetTransactionType(long ID);

        JsonResponse ChangeStatus(long ID, bool Status);

    }
}
