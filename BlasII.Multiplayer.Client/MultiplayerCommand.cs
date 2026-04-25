using Basalt.Framework.Networking.Client;
using BlasII.CheatConsole;
using BlasII.Multiplayer.Core;
using Il2CppTGK.Game;
using BlasII.CheatConsole;
using BlasII.ModdingAPI;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System;

namespace BlasII.Multiplayer.Client;

public class MultiplayerCommand : ModCommand
{
    public MultiplayerCommand() : base("multiplayer") { }
    private NetworkHandler networkHandler { get; set;}
    public static string PlayerName { get; set;}

    public void GetNetWorkHandler(NetworkHandler network)
    {
        networkHandler = network;
    }

    public override void Execute(string[] args)
    {
        switch (args[0])
        {
            case "connect":
                {
                    if (!ValidateParameterCount(args, 4) || networkHandler.isConnected)
                        return;
                    networkHandler.Connect(args[1], Int32.Parse(args[2]), new Models.RoomInfo("a", args[3], 1));
                    PlayerName = args[4];
                    break;
                }
            case "disconnect":
                {
                    if (!ValidateParameterCount(args, 1))
                        return;

                    networkHandler.Disconnect();
                    break;
                }
            default:
                {
                    WriteFailure("Unknown subcommand: " + args[0]);
                    break;
                }
        }
    }
}
