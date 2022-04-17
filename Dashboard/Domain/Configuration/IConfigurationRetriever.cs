namespace Domain.Configuration
{
    public interface IConfigurationRetriever
    {
        string? Get(string key);
    }
}