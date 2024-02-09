using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button   joinGameButton;

    private void Awake()
    {
       
        createGameButton.onClick.AddListener(() =>
        {
            Debug.Log("HOST");
            NetworkManager.Singleton.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
            
        });
        joinGameButton.onClick.AddListener(() =>
        {
            Debug.Log("CLIENT");
            NetworkManager.Singleton.StartClient();
            
        });
    }

    

}
