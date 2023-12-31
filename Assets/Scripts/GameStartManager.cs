using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : NetworkBehaviour
{

    public static GameStartManager Instance {  get; private set; }
    public Health health;
    public event EventHandler OnPlayerReadyChanged;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnpaused;
    public event EventHandler OnStateChanged;
  
    private enum State
    {
        WaitingToStart,
        GamePlaying,
        GameOver,
        CountdownToStart,
    }

    [SerializeField] private Transform hostPrefab;
    

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private bool isLocalGamePaused = false;
    private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    private bool isPlayerReady;
    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPausedDictionary;
    

    private void Awake()
    {
        Instance = this;
        health = GetComponent<Health>();
        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPausedDictionary = new Dictionary<ulong, bool>();

    }

  

    private void Start()
    {
        //GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        //GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }


    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        isGamePaused.OnValueChanged += isGamePaused_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;

        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform;

            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                // Spawn local player at a random position within the room
                playerTransform = Instantiate(hostPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            }
            else
            {
                // Spawn non-local player at a random position within the room
                playerTransform = Instantiate(hostPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            }

            var networkObject = playerTransform.GetComponent<NetworkObject>();
            networkObject.SpawnAsPlayerObject(clientId, clientId == NetworkManager.Singleton.LocalClientId);

            Debug.Log($"Player spawned. Is local player: {networkObject.IsLocalPlayer}");
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Replace this with logic to get random spawn positions within the room
        float randomX = UnityEngine.Random.Range(-11, 9);
        float randomY = UnityEngine.Random.Range(0.5f, 1.5f);
        float randomZ = UnityEngine.Random.Range(-14, 4);

        return new Vector3(randomX, randomY, randomZ);
    }


    private void isGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (isGamePaused.Value)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state.Value == State.WaitingToStart)
        {
            isPlayerReady = true;
            OnPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
           
        }
    }
   

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        if (!IsServer) 
        { 
            return;
        }

        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
            countdownToStartTimer.Value -= Time.deltaTime;
            if (countdownToStartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                if (health.currentHealth <= 0f)
                {
                    state.Value = State.GamePlaying;

                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsPlayerReady()
    {
        return isPlayerReady;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer.Value;
    }

    public void TogglePauseGame()
    {
        isLocalGamePaused = !isLocalGamePaused;
        if (!isLocalGamePaused)
        {
            PauseGameServerRpc();

            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnpauseGameServerRpc();
            
            OnLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePauseState();

    }
    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;

        TestGamePauseState();
    }

    private void TestGamePauseState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
            {
                // Should pause all players
                isGamePaused.Value = true;
                return;
            }
        }

        // no one should be paused
        isGamePaused.Value = false;
    }
}
