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
    public ConnectionInfo(string serverIp, string roomName, string playerName, int port, byte teamNumber)
    {
        ServerIp = serverIp ?? string.Empty;
        RoomName = roomName ?? string.Empty;
        PlayerName = playerName ?? string.Empty;
        ServerPort = port;
        TeamNumber = teamNumber;
    }

    /// <summary>
    /// Creates a new connection info with default parameters
    /// </summary>
    public ConnectionInfo()
    {
        ServerIp = $"{Protocol.DEFAULT_IP}";
        RoomName = string.Empty;
        PlayerName = string.Empty;
        ServerPort = Protocol.DEFAULT_PORT;
        TeamNumber = 1;
    }

    /// <summary>
    /// The ip address of the server
    /// </summary>
    public string ServerIp { get; }

    /// <summary>
    /// The name of the room
    /// </summary>
    public string RoomName { get; }

    /// <summary>
    /// The name of the player
    /// </summary>
    public string PlayerName { get; }

    /// <summary>
    /// The port
    /// </summary>
    public int ServerPort { get; }

    /// <summary>
    /// The team number (1-8)
    /// </summary>
    public byte TeamNumber { get; }
}
