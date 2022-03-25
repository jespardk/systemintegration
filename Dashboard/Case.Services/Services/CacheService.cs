using System.Runtime.Caching;

namespace Case.Services
{
    public class CacheService
    {
        private static MemoryCache? _memoryCache;
        public static MemoryCache? MemoryCache
        {
            get
            {
                if (_memoryCache == null)
                {
                    Initialize();
                }

                return _memoryCache;
            }
        }

        public static void ClearAllCache() => Initialize();

        private static void Initialize()
        {
            _memoryCache = new MemoryCache("Case.Services.SharedCache");
            Console.WriteLine("Cache: All cleared");
        }
    }
}