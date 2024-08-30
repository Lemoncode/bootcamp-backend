using Microsoft.AspNetCore.SignalR;

namespace SignalRMessagingTourOfHeroes.Hubs
{
    public class SignalRHub : Hub
    {

        const string CLIENT_METHOD = "SendToTheClient";

        public void SendToTheServer(string message)
        {
            Clients.All.SendAsync(CLIENT_METHOD, $"{Context.ConnectionId}: {message}");
            // Clients.Caller.SendAsync(CLIENT_METHOD, $"{Context.ConnectionId}: {message}");

            // var important = false;

            // if (message.Contains("updated") || message.Contains("added") || message.Contains("delete"))
            // {
            //     important = true;
            //     Clients.All.SendAsync(CLIENT_METHOD, $"{Context.ConnectionId}: {message}", important);
            // }
            // else
            // {
            //     Clients.Caller.SendAsync(CLIENT_METHOD, $"{Context.ConnectionId}: {message}", important);
            // }
        }

        public override Task OnConnectedAsync()
        {
            Clients.Others.SendAsync(CLIENT_METHOD, $"{Context.ConnectionId} joined");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.Others.SendAsync(CLIENT_METHOD, $"{Context.ConnectionId} left");

            return base.OnDisconnectedAsync(exception);
        }
    }
}