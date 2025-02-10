using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private UIDocument document;
    private Button startButton;
    private Button settingsButton;
    private Button exitButton;
    public GameObject SettingsUI;
    public GameObject PlayerSelectUI;
    private void Start()
    {
        document = GetComponent<UIDocument>();
        startButton = document.rootVisualElement.Q("startButton") as Button;
        settingsButton = document.rootVisualElement.Q("settingsButton") as Button;
        exitButton = document.rootVisualElement.Q("exitButton") as Button;
        startButton.RegisterCallback<ClickEvent>(OnStartClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        exitButton.RegisterCallback<ClickEvent>(OnExitClick);
    }
    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        startButton = document.rootVisualElement.Q("startButton") as Button;
        settingsButton = document.rootVisualElement.Q("settingsButton") as Button;
        exitButton = document.rootVisualElement.Q("exitButton") as Button;
        startButton.RegisterCallback<ClickEvent>(OnStartClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        exitButton.RegisterCallback<ClickEvent>(OnExitClick);
    }

    private void OnDisable()
    {
        startButton.UnregisterCallback<ClickEvent>(OnStartClick);
        settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        exitButton.UnregisterCallback<ClickEvent>(OnExitClick);
    }
    private void OnStartClick(ClickEvent evt)
    {
        // print("Start button pressed");
        gameObject.SetActive(false);
        PlayerSelectUI.SetActive(true);
    }
    private void OnSettingsClick(ClickEvent evt)
    {
        // print("Settings button pressed");
        gameObject.SetActive(false);
        SettingsUI.SetActive(true);
    }
    private void OnExitClick(ClickEvent evt)
    {
        // print("Exit button pressed");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
