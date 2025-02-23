using Microsoft.AspNetCore.SignalR;
using ClanChat.Core.DTOs.Message;

namespace ClanChat.Helpers
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(MessageDTO msg)
        {
            await Clients.Group($"clan-{msg.Sender.Clan.Id}").SendAsync("ReceiveNewMessage", msg);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
