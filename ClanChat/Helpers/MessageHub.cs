using Microsoft.AspNetCore.SignalR;
using ClanChat.Core.DTOs.Message;
using ClanChat.Abstractions.Message;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using CSharpFunctionalExtensions;

namespace ClanChat.Helpers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageHub : Hub
    {
        IMessageService _messageService;
        public MessageHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(CreateMessageDTO msg)
        {
            var userClaim = Context.User;
            var sendResult = await _messageService.SendMessageAsync(userClaim, msg);
            if (sendResult.IsFailure)
            {
                await Clients.Caller.SendAsync("SendMessageFailed", sendResult.Error);
            }
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
