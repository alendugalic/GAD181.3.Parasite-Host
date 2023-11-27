using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseMessageUI : MonoBehaviour
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
        
    }

    private void GameMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        Show();
        messageText.text = NetworkManager.Singleton.DisconnectReason;
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
