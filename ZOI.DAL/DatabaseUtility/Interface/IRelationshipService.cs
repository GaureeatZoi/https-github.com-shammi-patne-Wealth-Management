using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IRelationshipService
    {
        JsonResponse AddUpdate(Relationship model);
        Relationship GetData(int ID);
        bool IsExsits(string name, int ID);
        JsonResponse Summary();
        JsonResponse Deactivate(int ID, bool Status);
    }
}
