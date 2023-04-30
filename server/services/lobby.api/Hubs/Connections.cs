using lobby.api.Extensions;

namespace lobby.api.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public static class Connections
    {
        /// <summary>
        /// 
        /// </summary>
        public static ConnectionMapping<string> LobbyListConnections { get; } = new();

        /// <summary>
        /// 
        /// </summary>
        public static ConnectionMapping<string> LobbyConnections { get; } = new();
    }
}
