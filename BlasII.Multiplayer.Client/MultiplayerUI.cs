using BlasII.CheatConsole;
using BlasII.Framework.UI;
using BlasII.ModdingAPI.Helpers;
using BlasII.ModdingAPI.Input;
using Il2CppTGK.Game;
using Il2CppTMPro;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using BlasII.ModdingAPI;


namespace BlasII.Multiplayer.Client;

public class MultiplayerUI
{

    private bool _showHelp = true;
    private bool _showInfo = true;
    private string _currentIP = string.Empty;
    private string _currentPort = string.Empty;
    private string _currentNametag = string.Empty;
    private string _currentTeam = string.Empty;
    private TextMeshProUGUI displayedIP;
    private TextMeshProUGUI displayedPort;
    private TextMeshProUGUI displayedNametag;
    private TextMeshProUGUI displayedTeam;
    private RectTransform backIP;
    private TextMeshProUGUI _infoText;

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

    private string ProcessKeyInput(string storage, TextMeshProUGUI display)
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
        display.text = key;
        ModLog.Info($"{key}, {display.text}, after");
        display.color = Color.white;
        return key;
    }

    public void LateUpdate()
    {
        if (Input.inputString.Length > 0 && !_showHelp)
            _currentIP = ProcessKeyInput(_currentIP, displayedIP);
        //ProcessKeyInput(_currentPort, displayedPort);
        //ProcessKeyInput(_currentNametag, displayedNametag);
        //ProcessKeyInput(_currentTeam, displayedTeam);
        //if (_ip.GetKeyDown("ToggleMulti"))
        if (UnityEngine.Input.GetKeyDown(KeyCode.F9)) {
            if (_showHelp)
                displayedIP.text = _currentIP;
            _showHelp = !_showHelp;
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Backslash))
        {
            _showInfo = !_showInfo;
            SetTextVisibility(_showInfo);
        }

        if (!_showHelp && CoreCache.PlayerSpawn.PlayerInstance != null)
            UpdateTextFill();
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
        displayedIP.text = "";
    }
    private void UpdateTextFill()
    {
        var sb = new StringBuilder();

        // IP
        sb.AppendLine($"IP: ");

        // Port
        sb.AppendLine($"Port: ");

        // Room
        sb.AppendLine($"Room: ");

        // Nametag
        sb.AppendLine($"Nametag: ");

        // Team
        sb.AppendLine($"Team: ");


        _infoText.text = sb.ToString();
    }

    private void SetTextVisibility(bool visible)
    {
        if (_infoText == null)
            CreateText();

        _infoText.gameObject.SetActive(visible);
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
        displayedIP = UIModder.Create(new RectCreationOptions()
        {
            Name = "IP",
            Parent = UIModder.Parents.GameLogic,
            Size = new Vector2(400, 35),
            Pivot = new Vector2(0, 1),
            Position = new Vector2(80, -872),
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
            Name = "CheatConsole",
            Pivot = Vector2.zero,
            Position = new Vector2(80, -872),
            Size = new Vector2(400, 35),
            XRange = Vector2.zero,
            YRange = Vector2.zero,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(0.15f, 0.15f, 0.15f, 0.9f)
        }).rectTransform;
    }

    // size of text: 25 px heigh
    // 15 px between lines
}