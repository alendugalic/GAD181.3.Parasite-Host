using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{

    private void Start()
    {
        Hide();
        GameMultiplayer.Instance.onTryingToJoinGame += GameMultiplayer_OnTryingToJoinGame;
        GameMultiplayer.Instance.onFailedToJoinGame += GameStartManager_OnFailedToJoinGame;
       
    }

    private void GameStartManager_OnFailedToJoinGame(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameMultiplayer_OnTryingToJoinGame(object sender, EventArgs e)
    {
        Show();
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
        GameMultiplayer.Instance.onTryingToJoinGame -= GameMultiplayer_OnTryingToJoinGame;
        GameMultiplayer.Instance.onFailedToJoinGame -= GameStartManager_OnFailedToJoinGame;
    }
}
