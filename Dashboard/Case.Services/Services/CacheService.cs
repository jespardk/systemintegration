using System.Runtime.Caching;

namespace Case.Services
{
    public class CacheService
    {
        private static MemoryCache _memoryCache;

        public static MemoryCache MemoryCache
        {
            get {
                if (_memoryCache == null) Initialize();

                return _memoryCache; 
            }
            private set
            {
                _memoryCache = value;
            }
        }

        public static void ClearAllCache() => Initialize();

        private static void Initialize()
        {
            MemoryCache = new MemoryCache("Case.Services.SharedCache");
        }
    }
}