using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;
    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }
    private void Start()
    {
        Hide();
        GameMultiplayer.Instance.onFailedToJoinGame += GameMultiplayer_OnFailedToJoinGame;
        GameLobby.Instance.OnCreateLobbyStarted += GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed += GameLobby_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted += GameLobby_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed += GameLobby_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed += GameLobby_OnQuickJoinFailed;
    }

    private void GameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Could not find a Lobby to Quick Join!");
    }

    private void GameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to Join Lobby!");
    }

    private void GameLobby_OnJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Joining Lobby....");
    }

    private void GameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to create Lobby!");
    }

    private void GameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMessage("Creating Lobby....");
    }

    private void GameMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        if(NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed To Connect");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
        
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GameMultiplayer.Instance.onFailedToJoinGame -= GameMultiplayer_OnFailedToJoinGame;
    }
}
