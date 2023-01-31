using Microsoft.AspNetCore.SignalR;

namespace Lemoncode.Azure.Api.Hubs
{
    public class LikesHub : Hub
    {
        public Task NotifyAll(Notification notification) =>
            Clients.All.SendAsync("NotificationRecived", notification);

    }
    public record Notification(string Text, DateTime Date);
}
