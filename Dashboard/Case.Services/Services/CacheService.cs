using System.Runtime.Caching;

namespace Case.Services
{
    public class CacheService
    {
        public static MemoryCache MemoryCache = new MemoryCache("Case.Services.SharedCache");
    }
}