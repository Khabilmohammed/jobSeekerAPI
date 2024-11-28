using Microsoft.AspNetCore.SignalR;
using MimeKit;
using Org.BouncyCastle.Cms;
namespace jobSeeker.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendMessage(string sender, string receiver, string message)
        {
            
            await Clients.User(receiver).SendAsync("ReceiveMessage", sender, message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            await Clients.User(receiver).SendAsync("ReceiveNotification", "New Message", "You have received a new message.");

            
            await Clients.User(sender).SendAsync("ReceiveNotification", "Message Sent", "Your message was sent successfully.");
        }

        // Method to notify when a user joins the chat
        public override async Task OnConnectedAsync()
        {
            // Perform any actions on the user's connection (e.g., logging or tracking connections)
            await base.OnConnectedAsync();
        }

        // Method to handle when a user disconnects
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Handle any clean-up tasks here (e.g., removing from a connection tracking list)
            await base.OnDisconnectedAsync(exception);
        }
    }
}
