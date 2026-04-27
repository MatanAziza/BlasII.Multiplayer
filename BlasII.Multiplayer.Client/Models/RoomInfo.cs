
namespace BlasII.Multiplayer.Client.Models;

public class RoomInfo(string room, string player, string team)
{
    public string RoomName { get; } = room;

    public string PlayerName { get; } = player;

    public string Team { get; } = team;
}
