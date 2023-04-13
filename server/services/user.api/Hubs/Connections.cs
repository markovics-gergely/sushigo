using user.api.Extensions;

namespace user.api.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public static class Connections
    {
        /// <summary>
        /// 
        /// </summary>
        public static ConnectionMapping<string> UserConnections { get; } = new();
    }
}
