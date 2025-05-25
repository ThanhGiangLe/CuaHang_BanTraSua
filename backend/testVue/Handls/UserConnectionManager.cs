using System.Collections.Concurrent;

namespace testVue.Handls
{
    public class UserConnectionManager
    {
        private static readonly ConcurrentDictionary<int, HashSet<string>> _connections =
            new ConcurrentDictionary<int, HashSet<string>>();

        public void AddConnection(int userId, string connectionId)
        {
            var connections = _connections.GetOrAdd(userId, _ => new HashSet<string>());
            lock (connections)
            {
                connections.Add(connectionId);
            }
        }

        public void RemoveConnection(int userId, string connectionId)
        {
            if (_connections.TryGetValue(userId, out var connections))
            {
                lock (connections)
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        _connections.TryRemove(userId, out _);
                    }
                }
            }
        }

        public bool HasConnections(int userId)
        {
            return _connections.TryGetValue(userId, out var connections) && connections.Any();
        }
    }
}
