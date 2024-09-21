#region Cache Repository
using BankSystem.Application.Interface;
using BankSystem.SharedLibrarySolution.Logs;
using System.Runtime.Caching;
namespace BankSystem.Infrastructure.Repositories
{
    public class MyCacheService : IMyCacheService
    {
        private ObjectCache _memoryCache = MemoryCache.Default;
        public T GetData<T>(string key)
        {
            try
            {
                T item = (T)_memoryCache.Get(key);
                return item;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while getting Cached data");
            }
        }
        public object RemoveData(string key)
        {
            var result = true;

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var results = _memoryCache.Remove(key);
                }
                else
                    result = false;
                return result;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while removing Cached data");
            }
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationDate)
        {
            var result = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Set(key, value, expirationDate);
                }
                else
                {
                    result = false;
                }

                return result;

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while Caching data");
            }
        }
    }
}
#endregion