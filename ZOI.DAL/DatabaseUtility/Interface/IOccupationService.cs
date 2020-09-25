using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IOccupationService
    {
        bool IsExsits(string name, long ID);
        JsonResponse AddUpdate(OccupationMaster model);
        OccupationMaster GetData(long ID);

        JsonResponse Deactivate(long ID, bool Status);
        JsonResponse Summary();
    }
}
