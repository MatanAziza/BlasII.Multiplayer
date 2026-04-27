using BlasII.CheatConsole;
using BlasII.Framework.UI;
using BlasII.ModdingAPI.Helpers;
using BlasII.ModdingAPI.Input;
using Il2CppTGK.Game;
using Il2CppTMPro;
using System.Text;
using UnityEngine;
using System;
using BlasII.ModdingAPI;


namespace BlasII.Multiplayer.Client;

public class MultiplayerUI
{

    public bool _showHelp = true;
    public bool _showInfo = true;
    private string _currentIP = string.Empty;
    private string _currentPort = string.Empty;
    private string _currentNametag = string.Empty;
    private string _currentTeam = string.Empty;
    private TextMeshProUGUI displayedIP;
    private TextMeshProUGUI displayedPort;
    private TextMeshProUGUI displayedNametag;
    private TextMeshProUGUI displayedTeam;
    private RectTransform backIP;
    private RectTransform backPort;
    private RectTransform backNametag;
    private RectTransform backTeam;
    private TextMeshProUGUI _infoText;
    private int _selectedInput = 0;
    private NetworkHandler networkHandler { get; set; }
    public void GetNetWorkHandler(NetworkHandler network)
    {
        networkHandler = network;
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

    private string ProcessKeyInput(TextMeshProUGUI display)
    {
        var key = display.text;
        foreach (char c in Input.inputString)
        {
            // Backspace
            if (c == '\b')
            {
                if (key.Length > 0)
                    key = key[..^1];
            }
            // Regular character
            else
            {
                if (c != ' ' || key.Length > 0)
                    key += c;
            }
        }
        //if (key.Length > 15)
        //    key = key.Substring(0, 15);
        display.text = key;
        display.color = Color.white;
        return key;
    }

    public void LateUpdate()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.F9))
            _showHelp = !_showHelp;
        SwitchVisibleUI();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Backslash))
        {
            _showInfo = !_showInfo;
            _showHelp = true;
            SetTextVisibility(_showInfo);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (networkHandler.isConnected)
                return;
            networkHandler.Connect(_currentIP, Int32.Parse(_currentPort), new Models.RoomInfo("a", _currentNametag, 1));
            Multiplayer.PlayerName = _currentNametag;
            _showHelp = true;
        }

        if (Input.inputString.Length > 0 && !_showHelp)
        {
            if (_selectedInput == 0)
                _currentIP = ProcessKeyInput(displayedIP);
            else if (_selectedInput == 1)
                _currentPort = ProcessKeyInput(displayedPort);
            else if (_selectedInput == 2)
                _currentNametag = ProcessKeyInput(displayedNametag);
            else if (_selectedInput == 3)
                _currentTeam = ProcessKeyInput(displayedTeam);
        }
        if (!_showHelp && _showInfo && CoreCache.PlayerSpawn.PlayerInstance != null)
        {
            UpdateTextFill();
            if (Input.GetKeyDown(KeyCode.DownArrow) && _selectedInput < 3)
                _selectedInput++;
            else if (Input.GetKeyDown(KeyCode.UpArrow) && _selectedInput > 0)
                _selectedInput--;
        }
            if (_showHelp && CoreCache.PlayerSpawn.PlayerInstance != null)
            UpdateTextHelp();
    }

    //private void OnEnable()
    //{
    //    _ip.InputBlocked = true;
    //}

    //private void OnDisable()
    //{
    //    _ip.InputBlocked = false;
    //}

    private void UpdateTextHelp()
    {
        var sb = new StringBuilder();

        // How to Connect String
        sb.AppendLine($"Multiplayer: Press F9");

        _infoText.text = sb.ToString();
    }
    private void UpdateTextFill()
    {
        var sb = new StringBuilder();

        // IP
        sb.AppendLine($"IP: ");

        // Port
        sb.AppendLine($"Port: ");

        // Nametag
        sb.AppendLine($"Nametag: ");

        // Team
        sb.AppendLine($"Team: ");


        _infoText.text = sb.ToString();
    }

    private void SetTextVisibility(bool visible)
    {
        if (_infoText == null)
        {
            CreateText();
            SwitchVisibleUI();
        }

        _infoText.gameObject.SetActive(visible);
    }

    private void SwitchVisibleUI()
    {
        displayedIP.gameObject.SetActive(!_showHelp);
        displayedPort.gameObject.SetActive(!_showHelp);
        displayedNametag.gameObject.SetActive(!_showHelp);
        displayedTeam.gameObject.SetActive(!_showHelp);
        backIP.gameObject.SetActive(!_showHelp && _selectedInput == 0);
        backPort.gameObject.SetActive(!_showHelp && _selectedInput == 1);
        backNametag.gameObject.SetActive(!_showHelp && _selectedInput == 2);
        backTeam.gameObject.SetActive(!_showHelp && _selectedInput == 3);
    }

    private void CreateText()
    {
        _infoText = UIModder.Create(new RectCreationOptions()
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
        backIP = UIModder.Create(new RectCreationOptions()
        {
            Name = "IPBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(285, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(75, -912),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        displayedIP = UIModder.Create(new RectCreationOptions()
        {
            Name = "IP",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(400, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(80, -912),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddText(new TextCreationOptions()
        {
            Alignment = TextAlignmentOptions.BottomLeft,
            FontSize = 40,
            WordWrap = false,
        });
        backPort = UIModder.Create(new RectCreationOptions()
        {
            Name = "PortBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(255, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(105, -952),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        displayedPort = UIModder.Create(new RectCreationOptions()
        {
            Name = "Port",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(370, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(110, -952),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddText(new TextCreationOptions()
        {
            Alignment = TextAlignmentOptions.BottomLeft,
            FontSize = 40,
            WordWrap = false,
        });
        backNametag = UIModder.Create(new RectCreationOptions()
        {
            Name = "NametagBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(200, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(160, -992),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        displayedNametag = UIModder.Create(new RectCreationOptions()
        {
            Name = "Nametag",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(370, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(165, -992),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddText(new TextCreationOptions()
        {
            Alignment = TextAlignmentOptions.BottomLeft,
            FontSize = 40,
            WordWrap = false,
        });
        backTeam = UIModder.Create(new RectCreationOptions()
        {
            Name = "TeamBack",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(235, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(125, -1032),
            XRange = Vector2.zero,
            YRange = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
        displayedTeam = UIModder.Create(new RectCreationOptions()
        {
            Name = "Team",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(370, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(130, -1032),
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