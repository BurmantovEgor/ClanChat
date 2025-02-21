using Microsoft.AspNetCore.SignalR;
using ClanChat.Core.DTOs.Message;

namespace ClanChat.Helpers
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(MessageDTO msg)
        {
            await Clients.Group($"clan-{msg.User.Clan.Id}").SendAsync("ReceiveNewMessage", msg);
        }

        // Метод для подключения к группе
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Метод для выхода из группы (если нужно)
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
