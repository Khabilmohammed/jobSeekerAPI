﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace jobSeeker.SingleR
{
    [Authorize]
    public class PresenceHub:Hub
    {
        private readonly PresenceTracker _tracker;
        public PresenceHub(PresenceTracker presenceTracker)
        {
            _tracker = presenceTracker;
        }
        public override async Task OnConnectedAsync()
        {
            await _tracker.UserConnected(Context.User?.Identity?.Name,Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User?.Identity?.Name);

            var currentUsers=await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers",currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            await _tracker.UserDisconnected(Context.User?.Identity?.Name, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User?.Identity?.Name);

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }


       
    }
}
