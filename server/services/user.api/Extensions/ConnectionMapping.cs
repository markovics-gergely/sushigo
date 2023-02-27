namespace user.api.Extensions
{
    /// <summary>
    /// In-memory storage for connections
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConnectionMapping<T> where T : notnull
    {
        /// <summary>
        /// Storage variable for connections
        /// </summary>
        private readonly Dictionary<T, HashSet<string>> _connections = new();

        /// <summary>
        /// Get count of connections groups
        /// </summary>
        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        /// <summary>
        /// Add new connection by group id
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string>? connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        /// <summary>
        /// Get list of connections
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string>? connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Get Group exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(T key)
        {
            return _connections.ContainsKey(key);
        }

        /// <summary>
        /// Remove connection from key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string>? connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}
