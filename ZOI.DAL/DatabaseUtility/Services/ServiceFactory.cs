using System;
using System.Collections.Generic;
using System.Text;
using ZOI.BAL.DBContext;
using ZOI.DAL.DatabaseUtility.Interface;

namespace ZOI.DAL.DatabaseUtility.Services
{
    public sealed class ServiceFactory : IDisposable, IServiceFactory
    {
        private Dictionary<string, object> ___services = new Dictionary<string, object>();
        
        protected DatabaseContext dBContext;

        public ServiceFactory(DatabaseContext _dBContext)
        {
            this.dBContext = _dBContext;
        }

        public T GetService<T>() where T : BaseService
        {

            if (!___services.ContainsKey(typeof(T).Name))
            {
                ___services.Add(typeof(T).Name, (T)Activator.CreateInstance(typeof(T), dBContext));
            }

            return (T)___services[typeof(T).Name];
        }

        public void Dispose()
        {
            ___services.Clear();
            ___services = null;
        }

    }
}
