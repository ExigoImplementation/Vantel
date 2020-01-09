namespace Common.Models
{
    public interface IIdentityCacheable
    {
        string CacheKey { get; set; }

        void Initialize(int customerID);
        void RefreshCache();
    }
}