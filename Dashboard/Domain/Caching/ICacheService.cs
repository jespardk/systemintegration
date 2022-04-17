namespace Domain.Caching
{
    public interface ICacheService
    {
        void ClearAllCache();
        T Get<T>(string key) where T : class;
        void Set(string key, object value, int seconds = 120);
    }
}