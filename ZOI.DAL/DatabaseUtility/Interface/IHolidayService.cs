using System.Collections.Generic;
using ZOI.BAL.Models;


namespace ZOI.BAL.Services.Interface
{
    public interface IHolidayService
    {
        JsonResponse Summary();
        public IEnumerable<Holidays> ListAll();
        Holidays GetData(long ID);
        public bool IsExsits(string name, long ID);
        public JsonResponse Deactivate(long ID, bool Status);
        public JsonResponse AddUpdate(Holidays model);
      
      
    }
}
