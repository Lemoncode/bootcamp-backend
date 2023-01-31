using Lemoncode.Azure.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Lemoncode.Azure.Api.Services
{
    public class LikesService
    {
        private readonly IHubContext<LikesHub> hubContext;

        public LikesService(IHubContext<LikesHub> hubContext) =>
            hubContext = hubContext;

        public Task SendNotificationAsync(Notification notification) =>
            notification is not null
                ? hubContext.Clients.All.SendAsync("NotificationReceived", notification)
                : Task.CompletedTask;
    }
}
