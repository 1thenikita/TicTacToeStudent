using Microsoft.AspNetCore.SignalR;
using TicTacToeStudent.Server.Entities;

namespace TicTacToeStudent.Server.Hubs;

public class GameHub : Hub
{
    public const string URL = "/hubs/game";
    public List<PlayerEntity> Players { get; set; } = new();

    public override async Task OnConnectedAsync()
    {
    }

    public async Task Register(string name)
    {
        Players.Add(new() { Name = name, ID = new(), ConnectionID = Context.ConnectionId });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Players.RemoveAll(x => x.ConnectionID == Context.ConnectionId);
    }
}