using API.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{

    [Authorize]
    public class PresenceHub:Hub
    {
        private readonly PresenceTracker _presenceTracker;

        public PresenceHub(PresenceTracker presenceTracker)
        {
            _presenceTracker = presenceTracker;
        }
        public override async Task OnConnectedAsync()
        {
           var isOnline= await _presenceTracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            if (isOnline)
            {
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
            }

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
          var offline=  await _presenceTracker.UserDisConnected(Context.User.GetUsername(),Context.ConnectionId);
            if (offline)
            {
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            }
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}
