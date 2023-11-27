using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameLobby : MonoBehaviour
{
    private Lobby joinedLobby;
    private float heartbeatTimer;

    public static GameLobby Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication()
    {
       if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(Random.Range(0, 100).ToString());

            await UnityServices.InitializeAsync(initializationOptions);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
       
    }

    private void Update()
    {
        HandleHeartbeat();
    }
    private void HandleHeartbeat()
    {
        if(IsLobbyHost())
        {
            heartbeatTimer = Time.deltaTime;
            if(heartbeatTimer <= 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }
    private bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, GameMultiplayer.MAX_Player_AMMOUNT, new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
            });

            NetworkManager.Singleton.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
        } catch(LobbyServiceException e) 
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoin()
    {
        try 
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            NetworkManager.Singleton.StartClient();
        } catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
       
    }
    public async void JoinWithCode(string lobbyCode)
    {

        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            NetworkManager.Singleton.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        
    }

    public Lobby GetLobby()
    {
        return joinedLobby;
    }
}
