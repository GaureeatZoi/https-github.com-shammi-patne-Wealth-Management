using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IProductTypeService
    {
        public JsonResponse AddUpdate(ProductTypeMaster model);

        public JsonResponse Summary();

        public JsonResponse Deactivate(long ID,bool Status);

        public ProductTypeMaster GetData(long ID);

        public bool IsExsits(string name,long ID);
        
        public bool IsCodeExstis(string Code, long ID);

    }
}
