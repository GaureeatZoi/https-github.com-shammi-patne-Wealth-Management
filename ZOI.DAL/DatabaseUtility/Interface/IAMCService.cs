using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IAMCService
    {

        //JsonResponse UploadLogo(AMCs modal);

        JsonResponse CheckImage(AMCs model);

        public JsonResponse AddUpdate(AMCs model);

        public JsonResponse Summary();

        public JsonResponse Deactivate(long ID, bool Status);

        public AMCs GetData(long ID);
        
        public IEnumerable<SelectListItem> RTAsList();

        public bool IsExsits(string name, long ID);

    }
}
