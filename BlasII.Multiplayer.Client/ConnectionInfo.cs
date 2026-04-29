using Newtonsoft.Json;
using BlasII.Multiplayer.Core;

namespace BlasII.Multiplayer.Client;

/// <summary>
/// The data used to connect to the server
/// </summary>
public readonly struct ConnectionInfo
{
    /// <summary>
    /// Creates a new connection info
    /// </summary>
    [JsonConstructor]
    public ConnectionInfo(string serverIp, int serverPort, string playerName, string teamNumber, string roomName)
    {
        ServerIp = serverIp;
        ServerPort = serverPort;
        PlayerName = playerName;
        TeamNumber = teamNumber;
        RoomName = roomName;
    }

    /// <summary>
    /// Creates a new connection info with default parameters
    /// </summary>
    public ConnectionInfo()
    {
        ServerIp = string.Empty;
        ServerPort = 27051;
        PlayerName = string.Empty;
        TeamNumber = string.Empty;
        RoomName = string.Empty;
    }

    /// <summary>
    /// The ip address of the server
    /// </summary>
    public string ServerIp { get; }

    /// <summary>
    /// The port
    /// </summary>
    public int ServerPort { get; }

    /// <summary>
    /// The name of the player
    /// </summary>
    public string PlayerName { get; }

    /// <summary>
    /// The team number (1-8)
    /// </summary>
    public string TeamNumber { get; }

    /// <summary>
    /// The name of the room
    /// </summary>
    public string RoomName { get; }
}
