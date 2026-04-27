using BlasII.CheatConsole;
using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.ModdingAPI.Input;
using Il2CppTGK.Game;
using Il2CppTMPro;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking.Types;


namespace BlasII.Multiplayer.Client;

public class MultiplayerUI
{

    public bool _showHelp = true;
    public bool _showInfo = true;
    private bool _isConnected = false;

    //Stored values between F9's
    private string _currentIP = string.Empty;
    private string _currentPort = string.Empty;
    private string _currentNametag = string.Empty;
    private string _currentTeam = string.Empty;

    //Displayed boxes where to fill
    private RectTransform backIP;
    private RectTransform backPort;
    private RectTransform backNametag;
    private RectTransform backTeam;
    private RectTransform backConnect;
    private RectTransform backDisconnect;
    private TextMeshProUGUI _connectInfo;

    //Where the user is typing
    private int _selectedInput = 0;


    private NetworkHandler networkHandler { get; set; }
    public void GetNetWorkHandler(NetworkHandler network)
    {
        networkHandler = network;
        _isConnected = network.isConnected;
    }

    public void SceneLoaded()
    {
        if (_showInfo && SceneHelper.GameSceneLoaded)
            SetTextVisibility(true);
    }

    public void SceneUnloaded()
    {
        if (_showInfo && SceneHelper.GameSceneLoaded)
            SetTextVisibility(false);
    }

    private string ProcessKeyInput(string display)
    {
        foreach (char c in Input.inputString)
        {
            // Backspace
            if (c == '\b')
            {
                if (display.Length > 0)
                    display = display[..^1];
            }
            // Regular character
            else
            {
                if ((c != '\r' && c != ' ' )|| display.Length > 0)
                    display += c;
            }
        }
        return display;
    }

    private void UpdateDisplay()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            _showHelp = !_showHelp;
            SwitchVisibleUI();
        }
    }

    private void ConnectManager()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && _selectedInput == 4 && _showInfo && !_showHelp)
        {
            if (networkHandler.isConnected)
                networkHandler.Disconnect();
            //return;
            else
            {
                networkHandler.Connect(_currentIP, Int32.Parse(_currentPort), new Models.RoomInfo("a", _currentNametag, 1));
                Multiplayer.PlayerName = _currentNametag;
            }
            _isConnected = networkHandler.isConnected;
            _showHelp = true;
            SwitchVisibleUI();
        }
    }

    private void SelectInput()
    {
        if (!_showHelp && _showInfo && CoreCache.PlayerSpawn.PlayerInstance != null)
        {
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && _selectedInput < 4)
            {
                _selectedInput++;
                SwitchVisibleUI();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && _selectedInput > 0)
            {
                _selectedInput--;
                SwitchVisibleUI();
            }
            UpdateTextFill();
        }
    }

    private void InputFiller()
    {
        if (Input.inputString.Length > 0 && !_showHelp)
        {
            if (_selectedInput == 0)
                _currentIP = ProcessKeyInput(_currentIP);
            else if (_selectedInput == 1)
                _currentPort = ProcessKeyInput(_currentPort);
            else if (_selectedInput == 2)
                _currentNametag = ProcessKeyInput(_currentNametag);
            else if (_selectedInput == 3)
                _currentTeam = ProcessKeyInput(_currentTeam);
        }
    }

    public void LateUpdate()
    {
        // Switch Display
        UpdateDisplay();
        // CheatConsole Hide Help, buggy when showing ip, port, etc TEMP
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            _showInfo = !_showInfo;
            _showHelp = true;
            SetTextVisibility(_showInfo);
        }
        // Connect/Disconnect if "Connect" selected
        ConnectManager();
        // switch between inputs
        SelectInput();
        // process inputs based on selection
        InputFiller();
        // Change info to help message
        if (_showHelp && CoreCache.PlayerSpawn.PlayerInstance != null)
            UpdateTextHelp();
    }

    private void UpdateTextHelp()
    {
        var sb = new StringBuilder();
        // How to Connect
        sb.AppendLine($"Multiplayer: Press F9");

        _connectInfo.text = sb.ToString();
    }
    private void UpdateTextFill()
    {
        var sb = new StringBuilder();
        // IP
        sb.AppendLine($"IP: {_currentIP}");
        // Port
        sb.AppendLine($"Port: {_currentPort}");
        // Nametag
        sb.AppendLine($"Nametag: {_currentNametag}");
        // Team
        sb.AppendLine($"Team: {_currentTeam}");
        // Connect
        sb.AppendLine(networkHandler.isConnected ? "Disconnect" : "Connect");

        _connectInfo.text = sb.ToString();
    }

    private void SetTextVisibility(bool visible)
    {
        if (_connectInfo == null)
            CreateText();
        SwitchVisibleUI();
        _connectInfo.gameObject.SetActive(visible);
    }

    private void SwitchVisibleUI()
    {
        backIP.gameObject.SetActive(!_showHelp && _showInfo && _selectedInput == 0);
        backPort.gameObject.SetActive(!_showHelp && _showInfo && _selectedInput == 1);
        backNametag.gameObject.SetActive(!_showHelp && _showInfo && _selectedInput == 2);
        backTeam.gameObject.SetActive(!_showHelp && _showInfo && _selectedInput == 3);
        backConnect.gameObject.SetActive(!_showHelp && _showInfo && _selectedInput == 4 && !_isConnected);
        backDisconnect.gameObject.SetActive(!_showHelp && _showInfo && _selectedInput == 4 && _isConnected);
    }

    private void CreateText()
    {
        backIP = UIModder.Create(new RectCreationOptions()
        {
            Name = "IPBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(285, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(75, -872),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        backPort = UIModder.Create(new RectCreationOptions()
        {
            Name = "PortBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(255, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(105, -912),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        backNametag = UIModder.Create(new RectCreationOptions()
        {
            Name = "NametagBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(200, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(160, -952),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        backTeam = UIModder.Create(new RectCreationOptions()
        {
            Name = "TeamBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(235, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(125, -992),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        backConnect = UIModder.Create(new RectCreationOptions()
        {
            Name = "ConnectBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(120, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(15, -1032),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        backDisconnect = UIModder.Create(new RectCreationOptions()
        {
            Name = "DisconnectBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(170, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(15, -1032),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        _connectInfo = UIModder.Create(new RectCreationOptions()
        {
            Name = "Info Display",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(400, 200),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(20, -867),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddText(new TextCreationOptions()
        {
            Alignment = TextAlignmentOptions.BottomLeft,
            FontSize = 40,
            WordWrap = false,
        });
    }

    // size of text: 25 px heigh
    // 15 px between lines
}