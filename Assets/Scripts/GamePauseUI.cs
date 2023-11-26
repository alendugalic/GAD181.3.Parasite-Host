
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
//need to attatch networkObject to the inspector
//also add to network prefabs
// instantiate it in the script
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button mapButton;
    [SerializeField] private GameObject mainMap; // for the big map in menu
    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameStartManager.Instance.TogglePauseGame();
        });
        //mainMenuButton.onClick.AddListener(() =>
        //{
        //    ChildSceneLoader.Load(Loader.Scene.StartMenuScene);
        //});
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

    // public void BigMap()
    //{
         //Hide()
         //mainMap.SetActive(true)     this will show the canvas map
    //}

    //public void back()
    //{
          //Hide()
          //Show()
    //}
}
