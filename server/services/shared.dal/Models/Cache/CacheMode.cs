namespace shared.dal.Models.Cache
{
    public enum CacheMode
    {
        Get,
        Refresh,
        Bypass,
        Delete,
    }

    public static class CacheModeExtensions
    {
        public static bool IsFetch(this CacheMode mode)
        {
            return mode == CacheMode.Get;
        }
        public static bool IsStore(this CacheMode mode)
        {
            return mode == CacheMode.Get || mode == CacheMode.Refresh;
        }
    }
}
