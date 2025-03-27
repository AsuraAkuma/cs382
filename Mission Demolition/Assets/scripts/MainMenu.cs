using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument document;
    private Button startButton;
    private Button settingsButton;
    private Button quitButton;
    public GameObject settingsMenu;
    public GameObject Music;
    private Settings settings = new Settings();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Music.GetComponent<AudioSource>().volume = 0.2f * ((float)settings.volume / 100f);
        document = GetComponent<UIDocument>();
        startButton = document.rootVisualElement.Q("StartButton") as Button;
        settingsButton = document.rootVisualElement.Q("SettingsButton") as Button;
        quitButton = document.rootVisualElement.Q("QuitButton") as Button;
        startButton.RegisterCallback<ClickEvent>(OnStartClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        quitButton.RegisterCallback<ClickEvent>(OnQuitClick);
    }
    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        startButton = document.rootVisualElement.Q("StartButton") as Button;
        settingsButton = document.rootVisualElement.Q("SettingsButton") as Button;
        quitButton = document.rootVisualElement.Q("QuitButton") as Button;
        startButton.RegisterCallback<ClickEvent>(OnStartClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        quitButton.RegisterCallback<ClickEvent>(OnQuitClick);
    }

    private void OnDisable()
    {
        startButton.UnregisterCallback<ClickEvent>(OnStartClick);
        settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        quitButton.UnregisterCallback<ClickEvent>(OnQuitClick);
    }

    private void OnStartClick(ClickEvent evt)
    {
        Debug.Log("Start button clicked");
        // Show the game scene
        SceneManager.LoadScene("Game");
    }

    private void OnSettingsClick(ClickEvent evt)
    {
        Debug.Log("Settings button clicked");
        // Show the settings scene
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnQuitClick(ClickEvent evt)
    {
        Debug.Log("Quit button clicked");
        // Quit the game
        Application.Quit();
    }
}
