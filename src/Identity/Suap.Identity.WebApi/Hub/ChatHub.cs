using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections.Concurrent;



namespace Suap.Identity.Logic.Hub;
public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    static ConcurrentDictionary<string,string> _connections = new ();

    public override Task OnConnectedAsync()
    {

        _connections.TryAdd(Context.ConnectionId, Context.User?.Identity?.Name);
        Console.WriteLine("A Client Connected: " + Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _connections.TryRemove(Context.ConnectionId, out string val);
        Console.WriteLine("A client disconnected: " + Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task NewMessage(string user, string message)
    {
        await Clients.All.SendAsync("messageReceived", user, message);
     
    }

    
}
