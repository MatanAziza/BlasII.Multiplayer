using Basalt.Framework.Networking.Client;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Persistence;
using BlasII.ModdingAPI.Helpers;
using BlasII.Multiplayer.Client.Displays;
using BlasII.Multiplayer.Client.Nametags;
using BlasII.Multiplayer.Client.Storages;
using BlasII.Multiplayer.Core;
using Il2CppTGK.Game;
using BlasII.CheatConsole;

namespace BlasII.Multiplayer.Client;

public class Multiplayer : BlasIIMod, IGlobalPersistentMod<MultiplayerGlobalData>
{
    private readonly NetworkClient _client;

    public CompanionHandler CompanionHandler { get; }
    public MultiplayerCommand MultiCommand { get; }
    public MultiplayerUI MultiUI { get; set; }
    public NametagHandler NametagHandler { get; }
    public NetworkHandler NetworkHandler { get; }
    public PlayerHandler PlayerHandler { get; }

    public StatusDisplay StatusDisplay { get; }

    public AnimationStorage AnimationStorage { get; }
    public IconStorage IconStorage { get; }

    /// <summary>
    /// The most recent connectioninfo that should be used as the default
    /// </summary>
    public ConnectionInfo LastConnectionInfo { get; set; } = new();

    internal Multiplayer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        _client = new NetworkClient(new CoreSerializer());
        _client.OnClientConnected += TEMP_OnConnect;
        _client.OnClientDisconnected += TEMP_OnDisconnect;

        CompanionHandler = new CompanionHandler(_client);
        MultiCommand = new MultiplayerCommand();
        MultiUI = new MultiplayerUI();
        NametagHandler = new NametagHandler(_client);
        NetworkHandler = new NetworkHandler(_client);
        PlayerHandler = new PlayerHandler(_client);

        StatusDisplay = new StatusDisplay(_client);

        AnimationStorage = new AnimationStorage();
        IconStorage = new IconStorage(FileHandler);
    }

    protected override void OnInitialize()
    {
        AnimationStorage.Initialize();
    }

    protected override void OnLateUpdate()
    {
        if (!SceneHelper.GameSceneLoaded || CoreCache.PlayerSpawn.PlayerInstance == null)
            return;

        CompanionHandler.OnUpdate();
        NametagHandler.OnUpdate();
        NetworkHandler.OnUpdate();
        MultiCommand.GetNetWorkHandler(NetworkHandler);
        MultiUI.GetHandlers(NetworkHandler, InputHandler, LastConnectionInfo);
        PlayerHandler.OnUpdate();
        MultiUI.LateUpdate();

        StatusDisplay.OnUpdate();

        if (!MultiUI._canType || !MultiUI._showHelp)
            OnEnable();
        else
            OnDisable();
    }
    public void OnEnable()
    {
        InputHandler.InputBlocked = true;
    }

    public void OnDisable()
    {
        InputHandler.InputBlocked = false;
    }
    protected override void OnSceneLoaded(string sceneName)
    {
        if (sceneName == "MainMenu")
            return;

        CompanionHandler.OnEnterScene();
        NametagHandler.OnEnterScene();
        PlayerHandler.OnEnterScene();
        MultiUI.SceneLoaded();
    }

    protected override void OnSceneUnloaded(string sceneName)
    {
        CompanionHandler.OnLeaveScene();
        NametagHandler.OnLeaveScene();
        PlayerHandler.OnLeaveScene();
        MultiUI.SceneUnloaded();
    }

    // TODO: move these to the console popup + log
    private void TEMP_OnConnect(string ip)
    {
        ModLog.Info($"Connected to {ip}");
    }

    private void TEMP_OnDisconnect(string ip)
    {
        ModLog.Info($"Disconnected from {ip}");
    }

    /// <summary>
    /// Save last connection info
    /// </summary>
    public MultiplayerGlobalData SaveGlobal()
    {
        return new MultiplayerGlobalData()
        {
            LastConnection = LastConnectionInfo
        };
    }

    /// <summary>
    /// Load last connection info
    /// </summary>
    public void LoadGlobal(MultiplayerGlobalData data)
    {
        LastConnectionInfo = data.LastConnection;
    }

    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        provider.RegisterCommand(MultiCommand);
    }

    private const string SERVER = "localhost";
    private const int PORT = 27051;
    private const string PLAYER = "Phantom";
    private const string ROOM = "TEST";
    private const int TEAM = 1;

    public static string PlayerName { get; set; } = MultiplayerCommand.PlayerName;
}
