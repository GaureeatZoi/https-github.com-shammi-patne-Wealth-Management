using ZOI.BAL.Models;

namespace ZOI.BAL.Services.Interface
{
    public interface IRTAService
    {        

        public JsonResponse AddUpdate(RTAs model);

        public JsonResponse Summary();

        public JsonResponse Deactivate(long ID,bool Status);

        public RTAs GetData(long ID);

        public bool IsExsits(string name,long ID);

        

    }
}
