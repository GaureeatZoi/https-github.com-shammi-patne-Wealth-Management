using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IHoldingNatureService
    {
        bool IsExsits(string name, long ID);
        JsonResponse AddUpdate(HoldingNature model);
        HoldingNature GetData(long ID);

        JsonResponse Deactivate(long ID, bool Status);
        JsonResponse Summary();
    }
}
