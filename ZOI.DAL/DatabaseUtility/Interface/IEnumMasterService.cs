using System.Collections.Generic;
using ZOI.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZOI.BAL.ViewModels;
using ZOI.BAL;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IEnumMasterService
    {
        public JsonResponse Summary();
        public IEnumerable<ZOI.BAL.Models.Enum> ListAll();
        public ZOI.BAL.Models.Enum GetData(long ID);

        public bool IsExsitsEnumEcode(string EnumType, string EnumNumCode);
        public bool IsExsitsEnumEVal(string EnumType,  int EnumVal);

        public JsonResponse Deactivate(long ID, bool Status);
        public JsonResponse AddUpdate(ZOI.BAL.Models.Enum model);
    }
}
