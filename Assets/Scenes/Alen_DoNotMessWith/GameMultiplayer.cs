using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameMultiplayer : NetworkBehaviour
{
    public static GameMultiplayer Instance { get; private set; }

    public const int MAX_Player_AMMOUNT = 4;
    private const string PLAYER_PREF_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

    public event EventHandler onTryingToJoinGame;
    public event EventHandler onFailedToJoinGame;
    public event EventHandler onPlayerDataNetworkListChanged;

    private NetworkList<PlayerData> playerDataNetworkList;
    private string playerName;
 

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        playerName = PlayerPrefs.GetString(PLAYER_PREF_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100, 1000));

        playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
    }
    public string GetPlayerName()
    {
        return playerName;
    }
    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;

        PlayerPrefs.SetString(PLAYER_PREF_PLAYER_NAME_MULTIPLAYER, playerName);
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        onPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientDisconnectCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = playerDataNetworkList[i];
            if(playerData.clientId == clientId)
            {
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }
    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelectScene.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }
        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_Player_AMMOUNT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }
        connectionApprovalResponse.Approved = true;
    }
    public void StartClient()
    {
        onTryingToJoinGame?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        onFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }
}
