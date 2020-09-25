using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IInvestorCategoryService
    {
        public JsonResponse AddUpdate(InvestorCategorys model);

        public JsonResponse Summary();

        public JsonResponse Deactivate(long ID,bool Status);

        public InvestorCategorys GetData(long ID);

        public bool IsExsits(string name,long ID);

    }
}
