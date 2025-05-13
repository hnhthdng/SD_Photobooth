using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ServerAPI.Hubs
{
    public class TranferPhotoHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _sessionConnections = new();
        public TranferPhotoHub()
        {
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Client connected");
            await base.OnConnectedAsync();
        }

        public async Task<object> GetConnectionId(string sessionCode)
        {
            if(string.IsNullOrEmpty(sessionCode))
            {
                return new
                {
                    connectionId = Context.ConnectionId,
                    success = false,
                    message = "Session code cannot be null or empty.",
                };
            }

            if (_sessionConnections.TryGetValue(sessionCode, out var existingConnectionId))
            {
                if (existingConnectionId == Context.ConnectionId)
                {
                    return new
                    {
                        success = true,
                        connectionId = Context.ConnectionId,
                        message = "Connection already registered for this session.",
                        _sessionConnections
                    };
                }
                else
                {
                    return new
                    {
                        success = false,
                        message = $"Session code {sessionCode} is already in use by another device.",
                    };
                }
            }

            _sessionConnections.TryAdd(sessionCode, Context.ConnectionId);
            return new
            {
                success = true,
                connectionId = Context.ConnectionId,
                _sessionConnections
            };
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var sessionToRemove = _sessionConnections.FirstOrDefault(kv => kv.Value == Context.ConnectionId).Key;

            if (sessionToRemove != null)
            {
                _sessionConnections.Remove(sessionToRemove, out _);
            }

            Console.WriteLine("Client disconnected" + Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
