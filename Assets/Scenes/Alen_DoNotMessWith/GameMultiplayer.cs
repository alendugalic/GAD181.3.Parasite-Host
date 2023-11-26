using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class GameMultiplayer : NetworkBehaviour
{
    public static GameMultiplayer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        connectionApprovalResponse.Approved = true;
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
