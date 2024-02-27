using Microsoft.AspNetCore.SignalR;

namespace SignalRMessagingTourOfHeroes.Hubs
{
    public class SignalRHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.SendAsync("Send", $"{Context.ConnectionId}: {message}");
            // Clients.Caller.SendAsync("Send", $"{Context.ConnectionId}: {message}");

            // var important = false;

            // if (message.Contains("updated") || message.Contains("added") || message.Contains("delete"))
            // {
            //     important = true;
            //     Clients.All.SendAsync("Send", $"{Context.ConnectionId}: {message}", important);
            // }
            // else
            // {
            //     Clients.Caller.SendAsync("Send", $"{Context.ConnectionId}: {message}", important);
            // }
        }

        public override Task OnConnectedAsync()
        {
            Clients.Others.SendAsync("Send", $"{Context.ConnectionId} joined");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.Others.SendAsync("Send", $"{Context.ConnectionId} left");

            return base.OnDisconnectedAsync(exception);
        }
    }
}