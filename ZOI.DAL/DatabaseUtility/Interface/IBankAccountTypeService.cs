using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL;
using ZOI.BAL.Models;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IBankAccountTypeService
    {
        public long GetUserID();
        public JsonResponse AddUpdate(BankAccountType model);

        public JsonResponse Summary();

        public JsonResponse Deactivate(long ID, bool Status);

        public BankAccountType GetData(long ID);

        public bool IsExsits(string name, long ID);

    }
}
