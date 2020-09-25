using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IBankService
    {
        //JsonResponse UploadLogo(BankMaster modal);
        JsonResponse CheckImage(Bank modal);

        public JsonResponse AddUpdate(Bank model);

        public JsonResponse Summary();

        public JsonResponse Deactivate(long ID, bool Status);

        public Bank GetData(long ID);

        public bool IsExsits(string name, long ID);
    }
}
