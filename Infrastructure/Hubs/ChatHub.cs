using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task SendSystemMessage(string targetUserName, string message)
        {
            var target = await _userManager.FindByNameAsync(targetUserName);

            await Clients.User(target.Id)
                    .SendAsync("ReceiveSystemMessage", "System", targetUserName, message);
        }

        public async Task SendMessage(string targetUser, string message)
        {
            string senderId = Context.User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            string senderUsername = Context.User.FindFirst(x => x.Type.Equals(ClaimTypes.Name)).Value;

            if (string.IsNullOrEmpty(targetUser))
            {
                await Clients.All.SendAsync("ReceiveMessage", senderUsername, targetUser, message);
                return;
            }

            var target = await _userManager.FindByNameAsync(targetUser);

            if (target == null)
                await Clients.User(senderId).SendAsync("ReceiveMessage", "System", senderUsername, "User not found");
            else
                await Clients.Users(new List<string> { senderId, target.Id })
                    .SendAsync("ReceiveMessage", senderUsername, targetUser, message);
        }
    }
}