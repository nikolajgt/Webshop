
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;
using Webshop.Interface;
using Webshop.Models.DTO;

namespace Webshop.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationsHub : Hub<INotificiationsHub>
    {

        public override async Task OnConnectedAsync()
        {
            var userContext = GetUserinformation(Context);
            await Groups.AddToGroupAsync(userContext.ConnectionID, userContext.UserID);
            await SendUserinformationOut(userContext.ConnectionID, userContext.UserID);
            await base.OnConnectedAsync();
        }

        //Overrided hub method that removes "online" user
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var userContext = GetUserinformation(Context);
            await Groups.RemoveFromGroupAsync(userContext.ConnectionID, userContext.UserID);
            await base.OnDisconnectedAsync(ex);
        }

        //Instead of username, it should be userid that i get from JWT when logged in
        //So this can only be done when close to end when i implement JWT correctly

        private async Task SendUserinformationOut(string connectionid, string username)
        {
            await Clients.Client(connectionid).SendUserinformationOut(connectionid, username);
        }

        private HubCallerContextDTO GetUserinformation(HubCallerContext context)
        {
            var userid = context.User.Claims.Where(x => x.Type == ClaimTypes.Name).SingleOrDefault().Value;
            return new HubCallerContextDTO(Context.ConnectionId, userid);
        }
       

    }
}
