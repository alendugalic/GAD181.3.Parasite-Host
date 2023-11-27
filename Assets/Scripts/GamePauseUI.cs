
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button mapButton;
    [SerializeField] private Button optionsButton;
    
    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameStartManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        //mapButton.onClick.AddListener(() =>
        //{
        //    Hide();
        //    MapUI.Instance.Show(Show);
        //});
    }
    void Start()
    {
        GameStartManager.Instance.OnLocalGamePaused += GameStartManager_OnLocalGamePaused;
        GameStartManager.Instance.OnLocalGameUnpaused += GameStartManager_OnLocalGameUnpaused;
        Hide();
    }

    private void GameStartManager_OnLocalGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }
    private void GameStartManager_OnLocalGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);

        resumeButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    
    void Update()
    {
        
    }

    //public void Map()
    //{
    //    Hide()
    //     mainMap.SetActive(true)    /* this will show the canvas map*/
    //}

    //public void back()
    //{
       
    //}
}
