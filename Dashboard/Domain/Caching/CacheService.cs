using System.Runtime.Caching;

namespace Domain.Caching
{
    public class CacheService : ICacheService
    {
        private const int DefaultCacheTimeInSeconds = 120;
        private const string ApplicationCacheName = "DashboardApplication.SharedCache";
        private static MemoryCache? Instance;
        private static MemoryCache? MemoryCache
        {
            get
            {
                if (Instance == null)
                {
                    Initialize();
                }

                return Instance;
            }
        }

        public T Get<T>(string key) where T : class
        {
            if (MemoryCache?.Get(key) is T cachedItem)
            {
                Console.WriteLine($"{typeof(T).Name}: Read data cached");
                return cachedItem;
            }

            return null;
        }

        public void Set(string key, object value, int seconds = DefaultCacheTimeInSeconds)
        {
            var cachePolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(seconds)
            };

            MemoryCache?.Set(key, value, cachePolicy);
        }

        public void ClearAllCache() => Initialize();

        private static void Initialize()
        {
            Instance = new MemoryCache(ApplicationCacheName);
            Console.WriteLine("Cache: All cleared");
        }
    }
}