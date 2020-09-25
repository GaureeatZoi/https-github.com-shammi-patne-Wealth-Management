using ZOI.DAL.DatabaseUtility.Services;

namespace ZOI.DAL.DatabaseUtility.Interface
{
    public interface IServiceFactory
    {
        public T GetService<T>() where T : BaseService;
    }
}
