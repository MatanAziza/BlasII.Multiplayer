using BlasII.ModdingAPI.Persistence;
using BlasII.Multiplayer.Client;

namespace BlasII.Multiplayer.Client;

/// <summary>
/// The global save data for the multiplayer mod
/// </summary>
public class MultiplayerGlobalData : GlobalSaveData
{
    /// <summary>
    /// The most recent successful connection info
    /// </summary>
    public ConnectionInfo LastConnection { get; set; }
}
